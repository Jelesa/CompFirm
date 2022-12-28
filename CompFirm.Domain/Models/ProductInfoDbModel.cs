namespace CompFirm.Domain.Models
{
    public class ProductInfoDbModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public ulong GroupId { get; set; }

        public string GroupName { get; set; }

        public ulong ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        public string ProductType { get; set; }

        public int CharacteristicId { get; set; }

        public string CharacteristicName { get; set; }

        public string CharacteristicValue { get; set; }

        public string CharacteristicUnit { get; set; }
    }
}
