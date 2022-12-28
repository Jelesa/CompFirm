using System;
using System.Threading.Tasks;
using CompFirm.Domain.Exceptions;
using CompFirm.Dto.Products;
using Dapper;
using MySql.Data.MySqlClient;

namespace CompFirm.DataManagement.Concrete.Validation
{
    public static class ProductValidator
    {
        public static async Task ValidateCreate(MySqlConnection connection, ProductInfoDto product)
        {
            ValidateName(product);
            ValidatePrice(product);
            ValidateProductType(product);
            await ValidateGroup(connection, product);
            await ValidateManufacturer(connection, product);

            if (product.Characteristics != null && product.Characteristics.Count > 0)
            {
                foreach (var ch in product.Characteristics)
                {
                    await ValidateCharacteristic(connection, ch);
                }
            }
        }

        public static async Task ValidateEdit(MySqlConnection connection, ProductInfoDto product)
        {
            ValidateName(product);
            ValidatePrice(product);
            ValidateProductType(product);
            await ValidateProductExisting(connection, product);
            await ValidateGroup(connection, product);
            await ValidateManufacturer(connection, product);

            if (product.Characteristics != null && product.Characteristics.Count > 0)
            {
                foreach (var ch in product.Characteristics)
                {
                    await ValidateCharacteristic(connection, ch);
                }
            }
        }

        private static void ValidateName(ProductInfoDto product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                throw new BadRequestException("Не заполнено название продукта");
            }
        }

        private static void ValidatePrice(ProductInfoDto product)
        {
            if (product.Price <= 0)
            {
                throw new BadRequestException("Цена должна быть положительным числом");
            }
        }

        private static void ValidateProductType(ProductInfoDto product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductType))
            {
                throw new BadRequestException("Не заполнен вид товара");
            }
        }

        private static async Task ValidateProductExisting(MySqlConnection connection, ProductInfoDto product)
        {
            if (product.ProductId <= 0)
            {
                throw new BadRequestException("Не заполнен идентификатор товара");
            }

            var productId = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT `id` from `products` where `id`=@id",
                new
                {
                    id = product.ProductId
                });

            if (productId <= 0)
            {
                throw new BadRequestException("Нельзя обновить несуществующий продукт");
            }
        }

        private static async Task ValidateGroup(MySqlConnection connection, ProductInfoDto product)
        {
            if (product.Group == null || product.Group.Id <= 0)
            {
                throw new BadRequestException("Не заполнена группа");
            }

            var groupId = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT `id` from `groups` where `id`=@groupId",
                new
                {
                    groupId = product.Group.Id
                });

            if (groupId <= 0)
            {
                throw new BadRequestException("Указанная группа не существует");
            }
        }

        private static async Task ValidateManufacturer(MySqlConnection connection, ProductInfoDto product)
        {
            if (product.Manufacturer == null || product.Manufacturer.Id <= 0)
            {
                throw new BadRequestException("Не заполнен производитель");
            }

            var manufacturerId = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT `id` from `manufacturers` where `id`=@manId",
                new
                {
                    manId = product.Manufacturer.Id
                });

            if (manufacturerId <= 0)
            {
                throw new BadRequestException("Указанный производитель не существует");
            }
        }

        private static async Task ValidateCharacteristic(MySqlConnection connection, CharacteristicValueDto characteristic)
        {
            if (characteristic.CharacteristicId <= 0)
            {
                throw new BadRequestException("Не указана характеристика");
            }

            var characteristicName = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT `name` from `characteristics` where `id`=@id",
                new
                {
                    id = characteristic.CharacteristicId
                });

            if (string.IsNullOrWhiteSpace(characteristicName) 
                || !characteristicName.Equals(characteristic.CharacteristicName, StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException("Одна или несколько из указанных характеристик не существуют");
            }

            if (string.IsNullOrWhiteSpace(characteristic.Value))
            {
                throw new BadRequestException("Не указано значение одной или нескольких характеристик");
            }
        }

    }
}
