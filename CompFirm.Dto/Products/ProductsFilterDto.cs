using System.Collections.Generic;

namespace CompFirm.Dto.Products
{
    public class ProductsFilterDto
    {
        public string Name { get; set; }

        public int GroupId { get; set; }

        public string ProductType { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public Dictionary<string, string> Characteristics { get; set; }

        public ProductsFilterDto()
        {
            this.Limit = 20;
            this.Offset = 0;
        }
    }
}
