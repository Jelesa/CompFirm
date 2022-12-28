using CompFirm.Dto.Groups;
using CompFirm.Dto.Manufacturers;
using System.Collections.Generic;

namespace CompFirm.Dto.Products
{
    public class ProductInfoDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public GroupDto Group { get; set; }

        public ManufacturerDto Manufacturer { get; set; }

        public string ProductType { get; set; }

        public IReadOnlyCollection<CharacteristicValueDto> Characteristics { get; set; }
    }
}
