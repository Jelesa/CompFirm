namespace CompFirm.DataManagement.Constants
{
    public static class QueryTexts
    {
        public static class Users
        {
            public const string CreateUser = @"INSERT INTO `users`
                (`login`,
                `password`,
                `surname`,
                `name`,
                `patronymic`,
                `phone`) 
            VALUES
                (@login, @password, @family, @name, @surname, @phone);
            SELECT LAST_INSERT_ID()";

            public const string AddUserRole = @"INSERT INTO user_roles (id_role, id_user)
                                VALUES ((select id from roles where `name`=@roleName), @userId)";

            public const string GetUserCount = @"SELECT COUNT(*) FROM `users` WHERE `login`=@login OR `phone`=@phone";

            public const string GetUserInfo = @"select
login
from users u
where u.login = @login and u.password = @password";

            public const string GetUserWithRoles = @"select
u.login Login,
u.surname Surname,
u.name Name,
u.patronymic Patronymic,
u.phone Phone,
r.name RoleName
from users u
join user_roles ur on ur.id_user = u.id
join roles r on ur.id_role = r.id
where u.login = @login";

            public const string GetFullUserInfo = @"select
u.login Login,
u.surname Surname,
u.name Name,
u.patronymic Patronymic,
u.phone Phone,
u.password Password,
r.name RoleName
from users u
join user_roles ur on ur.id_user = u.id
join roles r on ur.id_role = r.id
where u.login = @login";

            public const string UpdateUserInfo = @"update users
set surname = @surname,
	`name` = @name,
	patronymic = @patronymic
where login = @login";

            public const string UpdatePassword = @"update users
set password = @password
where login = @login";
        }


        public static class Groups
        {
            public const string GetGroupById = @"select
id Id,
IFNULL(parent_group_id, -1) ParentGroupId,
`name` Name
                    from `groups` WHERE id=@id";

            public const string GetMainGroups = @"select id Id, `name` Name
                    from  `groups` where parent_group_id is null
                    ORDER BY `name`";

            public const string GetChildGroups = @"select
g.id Id,
g.`name` Name,
parentGroups.id ParentGroupId,
parentGroups.`name` ParentGroupName
from `groups` g
join `groups` parentGroups on g.parent_group_id = parentGroups.id
where parentGroups.id = @groupId
ORDER BY g.`name`";

            public const string GetProductTypes = @"select
DISTINCT
prch.value Name
from product_characteristics prch
join characteristics ch on prch.id_characteristic = ch.id and ch.Name='Вид товара'
join products p on prch.id_product = p.id
where p.id_group = @groupId";

            public const string GetAllGroups = @"select
g.id Id,
g.`name` Name,
parentGroups.id ParentGroupId,
parentGroups.`name` ParentGroupName
from `groups` g
left join `groups` parentGroups on g.parent_group_id = parentGroups.id";

            public const string DeleteGroupById = @"DELETE FROM `groups` WHERE id=@id";

            public const string GetCountGroupUse = @"select count(p.id) from products p where p.id_group = @id";

            public const string GetCountGroupWithName = @"select count(g.id) from `groups` g where g.`name` = @name and g.parent_group_id=NULLIF(@parentGroupId, -1)";

            public const string GetCountChildGroups = @"select count(g.id) from `groups` g where g.parent_group_id=@id";

            public const string UpdateGroupNameById = @"UPDATE `groups` SET name=@name, parent_group_id=NULLIF(@parentGroupId, -1) WHERE id=@id";

            public const string CreateGroup = @"insert into `groups` (`name`, parent_group_id) values (@name, NULLIF(@parentGroupId, -1))";
        }

        public static class Products
        {
            public const string GetPriceInterval = @"select
MIN(price) MinPrice,
MAX(price) MaxPrice
from products;";

            public const string InsertProductsWriteOff = @"insert into products_moving (id_product, action_date, count, id_moving_type)
select 
	id_product,
    current_timestamp(),
    -1 * count,
    (select id from products_moving_type where `name`='Выдача товара')
from request_items 
where id_request = @p_requestId;";
            
            public const string GetProductsOstatki = @"select
ostView.OST - ri.count
from request_items ri
join (select
pm.id_product,
IFNULL(SUM(pm.count), 0) OST
from request_items ri
left join products_moving pm on ri.id_product = pm.id_product
where ri.id_request = 1
GROUP BY pm.id_product) ostView on ri.id_product = ostView.id_product
where ri.id_request = @p_requestId";

            public const string GetReturnedProducts = @"select 
pm.id Id,
pm.action_date `Date`,
p.name ProductName,
pm.count Count
from products_moving pm
join products p on pm.id_product = p.id
where pm.id_moving_type = (select id 
							from products_moving_type pmt
                            where pmt.name = 'Возврат товара');";

            public const string DeleteReturnedProduct = @"delete from products_moving where id=@id;";

            public const string GetProductShortInfo = @"SELECT
                                                p.id Id,
                                                p.name ProductName,
                                                p.price Price,
                                                g.id GroupId,
                                                g.name GroupName
                                                FROM products p
                                                JOIN `groups` g on p.id_group = g.id
                                                ORDER BY p.`name`";

            public const string GetCountProductOrdered = @"select 
count(ri.id) 
from request_items ri
where ri.id_product = @id;";

            public const string GetCountProductMoving = @"select 
count(pm.id) 
from products_moving pm
where pm.id_product = @id";

            public const string DeleteProduct = @"DELETE FROM products WHERE id=@id";

            public const string GetProduct = @"SELECT
p.id ProductId,
p.name ProductName,
p.price Price,
p.image_url ImageUrl,
g.id GroupId,
g.`name` GroupName,
m.id ManufacturerId,
m.`name` ManufacturerName,
ptView.productType ProductType,
chView.CharacteristicId,
chView.CharacteristicName,
chView.CharacteristicValue,
chView.CharacteristicUnit
FROM products p
join `groups` g on p.id_group = g.id
join manufacturers m on p.id_manufacturer = m.id
left join (select
        pr.id productId,
        prch.value productType
    from products pr
    join product_characteristics prch on pr.id = prch.id_product
    join characteristics ch on prch.id_characteristic = ch.id and ch.name = 'Вид товара') ptView on ptView.productId = p.id
left join (select
        prch.id_product productId, 
        ch.id CharacteristicId,
        ch.name CharacteristicName,
        prch.value CharacteristicValue,
        ch.unit CharacteristicUnit
    from product_characteristics prch
    join characteristics ch on ch.id = prch.id_characteristic and ch.name != 'Вид товара') chView on chView.productId = p.id
WHERE p.id = @id";
        }

        public class Requests
        {
            public const string GetDraftRequestIdByLogin = @"select 
r.id requestID
from requests r
join users u on r.id_user = u.id
where GetRequestStatus(r.id) = 'Черновик' and u.login = @Login;";

            public const string GetRequestItemByProduct = @"
select ri.id from request_items ri
where ri.id_product = @productId and ri.id_request = @requestId ";

            public const string UpdateRequestItemCount = @"update request_items
set count = @countProduct
where request_items.id = @itemId";

            public const string InsertRequestItem = @"insert into request_items (count, id_request, id_product) 
values (@countProduct, @requestId, @productId)";

            public const string InsertRequest = @"insert into requests (creation_date, period_excution, id_user) 
values (current_timestamp(), current_timestamp(), (select id from users where users.login = @Login));
SELECT LAST_INSERT_ID()";

            public const string InsertRequestStatus = @"insert into request_status_journal (id_request, id_request_status)
value (@requestId, (select id from request_status where request_status.name = @Status))";

            public const string DeleteRequestItem = @"delete from request_items where id=@id";

            public const string GetCartItems = @"select 
p.id ProductId,
p.name ProductName,
p.image_url ProductImage,
p.price ProductPrice,
ri.count Count
from requests r
join request_items ri on ri.id_request = r.id
join products p on p.id = ri.id_product
join users u on r.id_user = u.id
where GetRequestStatus(r.id) = 'Черновик' and u.login = @Login";

            public const string UpdateRequest = @"update requests
set creation_date = current_timestamp(),
	period_excution = DATE_ADD(current_timestamp(), INTERVAL 10 DAY)
where requests.id = @id";

            public const string UpdateRequestStatus = @"insert into request_status_journal(id_request, id_request_status, status_date) values
((select 
r.id requestID
from requests r
join users u on r.id_user = u.id
where GetRequestStatus(r.id) = 'Черновик' and u.login = @Login), 
(select id from request_status where request_status.name = @status), current_timestamp())";

            public const string GetRequestCard = @"select
    r.id `Number`,
    r.creation_date `CreationDate`,
    r.period_excution `PeriodExecution`,
    u.login `UserName`,
    GetRequestStatus(r.id) `Status`
from requests r
join users u on r.id_user = u.id
where r.id = @requestId";

            public const string GetRequestItemsByRequest = @"select 
p.id ProductId,
p.name ProductName,
p.image_url ProductImage,
p.price ProductPrice,
ri.count Count
from requests r
join request_items ri on ri.id_request = r.id
join products p on p.id = ri.id_product
where r.id=@requestId";

            public const string GetRequestJournal = @"select 
rs.`name` StatusName,
rsj.status_date StatusDate
from requests r
join request_status_journal rsj on rsj.id_request = r.id
join request_status rs on rsj.id_request_status = rs.id
where r.id=@requestId
ORDER BY rsj.status_date";

            public const string CancelRequest = @"INSERT INTO request_status_journal (id_request_status, id_request)
VALUES ((select `id` from request_status WHERE `name`='Отменен'), @requestId)";

            public const string StatusUpdateRequestWithId = @"insert into request_status_journal(id_request, id_request_status, status_date) values
(@id, 
(select id from request_status where request_status.name = @status), current_timestamp())";

            public const string GetLastStatus = @"select
rs.`name`
from request_status_journal rsj
join request_status rs on rsj.id_request_status = rs.id
where rsj.id_request = @p_requestId
order by rsj.status_date DESC
LIMIT 1;";

        }

        public class Manufacturers
        {
            public const string GetManufacturers = @"select m.id Id, m.`name` `Name` from manufacturers m";

            public const string GetManufacturerByName = @"select m.id Id, m.`name` `Name` from manufacturers m where m.`name` = @name";

            public const string GetCountProducts = @"select COUNT(p.id) from products p where p.id_manufacturer = @id";
        }

        public class Characteristics
        {
            public const string GetCharacteristics = @"select
id Id,
`name` `Name`,
`unit` `Unit`
from characteristics ch
WHERE `name` != 'Вид товара'";

            public const string GetCharacteristicByName = @"select
id Id,
`name` `Name`,
`unit` `Unit`
from characteristics ch
WHERE `name` = @name";

            public const string GetCountProducts = @"select COUNT(pch.id) from product_characteristics pch where pch.id_characteristic = @id";
        }
    }
}
