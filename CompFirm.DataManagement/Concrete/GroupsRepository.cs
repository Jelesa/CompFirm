using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Exceptions;
using CompFirm.Dto.Groups;
using CompFirm.Dto.Users;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.SearchFilter;
using System.Data;
using CompFirm.DataManagement.Constants;
using CompFirm.Dto.SearchResult;
using MySql.Data.MySqlClient;

namespace CompFirm.DataManagement.Concrete
{
    public class GroupsRepository : IGroupsRepository
    {
        private const string RowCountParameterName = "p_totalRowCount";

        private readonly IDataAccess dataAccess;

        public GroupsRepository(
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<IReadOnlyCollection<GroupDto>> GetChildGroups(int id)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                return (await connection.QueryAsync<GroupDto>(QueryTexts.Groups.GetChildGroups,
                    new
                    {
                        groupId = id
                    })).AsList();
            }
        }

        public async Task<IReadOnlyCollection<GroupDto>> GetMainGroups()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                return (await connection.QueryAsync<GroupDto>(QueryTexts.Groups.GetMainGroups)).AsList();
            }
        }

        public async Task<IReadOnlyCollection<ProductTypeDto>> GetProductTypes(int id)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                return (await connection.QueryAsync<ProductTypeDto>(
                    QueryTexts.Groups.GetProductTypes,
                    new
                    {
                        groupId = id
                    })).AsList();
            }
        }

        public async Task<SearchResultDto<GroupDto>> GetGroups(BaseSearchFilterDto filter)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("p_limit", filter.Limit);
                parameters.Add("p_offset", filter.Offset);
                parameters.Add(RowCountParameterName, dbType: DbType.Int32, direction: ParameterDirection.Output);
                
                var dbRes = await connection.QueryAsync<GroupDto>("GROUPS_SEARCH", parameters, commandType: CommandType.StoredProcedure);

                int totalRowCount = parameters.Get<int>(RowCountParameterName);

                var dto = new SearchResultDto<GroupDto>
                {
                    Found = totalRowCount,
                    Result = dbRes.AsList()
                };

                return dto;
            }
        }

        public async Task<GroupDto> GetGroupById(int id, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на удаление категории");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<GroupDto>(QueryTexts.Groups.GetGroupById,
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task UpdateGroupById(GroupDto group, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на изменение категории");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                await this.ValidateDuplicate(connection, group);

                var childCount = await connection.QueryFirstOrDefaultAsync<int>(
                    QueryTexts.Groups.GetCountChildGroups,
                    new
                    {
                        id = group.Id
                    });

                if (childCount > 0 && group.ParentGroupId != -1)
                {
                    throw new BadRequestException("Невозможно изменить родительскую группу, т.к. в данной группе есть подгруппы");
                }

                await connection.QueryFirstOrDefaultAsync<GroupDto>(
                    QueryTexts.Groups.UpdateGroupNameById,
                    new
                    {
                        id = group.Id,
                        parentGroupId = group.ParentGroupId,
                        name = group.Name
                    });
            }
        }
        public async Task DeleteGroupById(int id, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на удаление категории");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var count = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Groups.GetCountGroupUse,
                        new
                        {
                            id = id
                        });

                    if (count > 0)
                    {
                        throw new BadRequestException("Категория не может быть удалена т.к. существуют товары или улуги данной категории.");
                    }

                    await connection.ExecuteAsync(QueryTexts.Groups.DeleteGroupById,
                        new
                        {
                            id = id
                        });

                    await transaction.CommitAsync();
                }

            }
        }

        public async Task CreateGroup(GroupDto group, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на удаление категории");
            }

            if (string.IsNullOrWhiteSpace(group.Name))
            {
                throw new BadRequestException("Не заполнено имя группы");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                await this.ValidateDuplicate(connection, group);

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await connection.ExecuteAsync(QueryTexts.Groups.CreateGroup,
                        new
                        {
                            name = group.Name,
                            parentGroupId = group.ParentGroupId
                        });

                    await transaction.CommitAsync();
                }

            }
        }

        private async Task ValidateDuplicate(MySqlConnection connection, GroupDto group)
        {
            var groupsCount = await connection.QueryFirstOrDefaultAsync<int>(
                QueryTexts.Groups.GetCountGroupWithName,
                new
                {
                    name = group.Name,
                    parentGroupId = group.ParentGroupId
                });

            if (groupsCount > 0)
            {
                throw new BadRequestException("Такая группа уже существует в данном разделе");
            }
        }
    }
}
