using System;
using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Constants;
using CompFirm.Dto.Products;
using CompFirm.Dto.RequestItems;
using CompFirm.Dto.Requests;
using CompFirm.Dto.Users;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CompFirm.DataManagement.Constants;
using CompFirm.Domain.Exceptions;
using CompFirm.Dto.SearchResult;
using CompFirm.Dto.Statuses;

namespace CompFirm.DataManagement.Concrete
{
    public class RequestsRepository : IRequestsRepository
    {
        private const string RowCountParameterName = "p_totalRowCount";

        private readonly IDataAccess dataAccess;

        public RequestsRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task AddItemCard(CartItemDto cartItem, PayloadDto payload)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var requestId = await this.GetDraftRequestId(connection, payload);

                    var itemID = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Requests.GetRequestItemByProduct,
                            new
                            {
                                requestId = requestId,
                                productId = cartItem.ProductId
                            });

                    if (itemID != 0)
                    {
                        await this.UpdateRequestItem(connection, cartItem, itemID);
                    }
                    else if (cartItem.Count != 0)
                    {
                        await connection.ExecuteAsync(QueryTexts.Requests.InsertRequestItem,
                           new
                           {
                               countProduct = cartItem.Count,
                               requestId = requestId,
                               productId = cartItem.ProductId
                           });
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<IReadOnlyCollection<RequestItemDto>> GetCartItems(PayloadDto payload)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var cartItems = await connection.QueryAsync<RequestItemDto>(QueryTexts.Requests.GetCartItems,
                    new
                    {
                        Login = payload.Login
                    });

                return cartItems.AsList();
            }
        }

        public async Task ConfirmRequest(PayloadDto payload)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var requestID = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Requests.GetDraftRequestIdByLogin,
                        new
                        {
                            Login = payload.Login
                        });

                    await connection.ExecuteAsync(QueryTexts.Requests.UpdateRequest,
                        new
                        {
                            id = requestID
                        });

                    await connection.ExecuteAsync(QueryTexts.Requests.UpdateRequestStatus,
                        new
                        {
                            Login = payload.Login,
                            status = RequestStatusNames.Created
                        });

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateRequestStatus(
            PayloadDto payload,
            UpdateRequestStatusDto requestStatus)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение данного запроса");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                await this.ValidateRequestStatus(connection, requestStatus);

                if (string.Compare(
                    requestStatus.StatusName,
                    RequestStatusNames.ReadyToRelease,
                    StringComparison.OrdinalIgnoreCase) == 0)
                {
                    await this.CheckProductsCount(connection, requestStatus.RequestId);
                }

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await connection.ExecuteAsync(QueryTexts.Requests.StatusUpdateRequestWithId,
                           new
                           {
                               id = requestStatus.RequestId,
                               status = requestStatus.StatusName
                           });

                    if (string.Compare(
                        requestStatus.StatusName,
                        RequestStatusNames.Gived,
                        StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        await this.WriteOffProducts(connection, requestStatus.RequestId);
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<SearchResultDto<RequestShortInfoDto>> SearchRequests(
            PayloadDto payload,
            RequestsFilterDto filter)
        {
            if (filter.AdminSearch && !payload.IsAdmin)
            {
                throw new BadRequestException("У вас нет прав выполнять поиск от имени администратора");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var parameters = GetParameters(payload, filter);

                var dbRes = await connection.QueryAsync<RequestShortInfoDto>("REQUESTS_SEARCH", parameters, commandType: CommandType.StoredProcedure);

                return new SearchResultDto<RequestShortInfoDto>
                {
                    Found = parameters.Get<int>(RowCountParameterName),
                    Result = dbRes.AsList()
                };
            }
        }

        public async Task<RequestFullInfoDto> GetRequestCard(int requestId)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var requestCard = await connection.QueryFirstOrDefaultAsync<RequestFullInfoDto>(
                    QueryTexts.Requests.GetRequestCard,
                    new
                    {
                        requestId = requestId
                    });

                if (requestCard == null)
                {
                    throw new BadRequestException("Заказ не найден");
                }

                requestCard.RequestItems = (await connection.QueryAsync<RequestItemDto>(
                    QueryTexts.Requests.GetRequestItemsByRequest,
                    new
                    {
                        requestId = requestId
                    })).AsList();

                requestCard.Sum = requestCard.RequestItems.Select(x => x.Count * x.ProductPrice).Sum();

                requestCard.Journal = (await connection.QueryAsync<RequestJournalItemDto>(
                    QueryTexts.Requests.GetRequestJournal,
                    new
                    {
                        requestId = requestId
                    }
                )).AsList();

                requestCard.Status = requestCard.Journal.LastOrDefault()?.StatusName ?? RequestStatusNames.Draft;

                return requestCard;
            }
        }

        public async Task CancelRequest(PayloadDto payload, int requestId)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var requestCard = await connection.QueryFirstOrDefaultAsync<RequestFullInfoDto>(
                    QueryTexts.Requests.GetRequestCard,
                    new
                    {
                        requestId = requestId
                    });

                if (requestCard == null)
                {
                    throw new BadRequestException("Заказ не найден");
                }

                if (!requestCard.UserName.Equals(payload.Login, StringComparison.OrdinalIgnoreCase)
                    && !payload.IsAdmin
                    || RequestStatusNames.UnableToCancelStatus.Contains(requestCard.Status))
                {
                    throw new BadRequestException("Вы не можете отменить данный заказ");
                }

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await connection.ExecuteAsync(
                        QueryTexts.Requests.CancelRequest,
                        new
                        {
                            requestId = requestId
                        });

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<IReadOnlyCollection<RequestStatusDto>> GetStatusList()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var dto = await connection.QueryAsync<RequestStatusDto>("SELECT id `Id`, `name` `Name` from request_status WHERE `name` != 'Черновик'");

                return dto.AsList();
            }
        }

        private async Task<int> GetDraftRequestId(MySqlConnection connection, PayloadDto payload)
        {
            var requestID = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Requests.GetDraftRequestIdByLogin,
                new
                {
                    Login = payload.Login
                });

            if (requestID == 0)
            {
                requestID = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Requests.InsertRequest,
                    new
                    {
                        Login = payload.Login
                    });

                await connection.ExecuteAsync(QueryTexts.Requests.InsertRequestStatus,
                    new
                    {
                        requestId = requestID,
                        Status = RequestStatusNames.Draft
                    });
            }

            return requestID;
        }

        private async Task UpdateRequestItem(
            MySqlConnection connection,
            CartItemDto cartItem,
            int requestItemId)
        {
            if (cartItem.Count == 0)
            {
                await connection.ExecuteAsync(QueryTexts.Requests.DeleteRequestItem,
                    new
                    {
                        id = requestItemId
                    });

                return;
            }

            await connection.ExecuteAsync(QueryTexts.Requests.UpdateRequestItemCount,
                new
                {
                    countProduct = cartItem.Count,
                    itemId = requestItemId
                });
        }

        private async Task WriteOffProducts(MySqlConnection connection, int requestId)
        {
            await connection.ExecuteScalarAsync(QueryTexts.Products.InsertProductsWriteOff,
                new
                {
                    p_requestId = requestId,
                    movingType = "Выдача товара"
                });
        }

        private async Task CheckProductsCount(MySqlConnection connection, int requestId)
        {
            var ost = await connection.QueryAsync<int>(QueryTexts.Products.GetProductsOstatki,
                new
                {
                    p_requestId = requestId
                });

            if (ost.Any(x => x < 0))
            {
                throw new BadRequestException("Невозможно выдать данный заказ т.к. на складе недостаточно товаров.");
            }
        }

        private async Task ValidateRequestStatus(MySqlConnection connection, UpdateRequestStatusDto requestStatus)
        {
            var currentStatus = await connection.QueryFirstOrDefaultAsync<string>(
                QueryTexts.Requests.GetLastStatus,
                new
                {
                    p_requestId = requestStatus.RequestId
                });

            var statusMap = RequestStatusNames.StatusRoadMap[currentStatus];

            if (!statusMap.Any(x => x.Equals(requestStatus.StatusName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BadRequestException("Данному заказу невозможно установить переданный статус");
            }
        }

        private static DynamicParameters GetParameters(
            PayloadDto payload,
            RequestsFilterDto filter)
        {
            var parameters = new DynamicParameters();

            parameters.Add("p_login", !filter.AdminSearch ? payload.Login : null);
            parameters.Add("p_searchString", filter.SearchString);
            parameters.Add("p_statusId", filter.Status);
            parameters.Add("p_limit", filter.Limit);
            parameters.Add("p_offset", filter.Offset);
            parameters.Add(RowCountParameterName, dbType: DbType.Int32, direction: ParameterDirection.Output);

            return parameters;
        }
    }
}
