using System;

namespace CompFirm.Dto.ProductsMoving
{
    public class ProductsMovingDto
    {
        public int Id { get; set; }

        public string MovingType { get; set; }

        public DateTime ActionDate { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Count { get; set; }
    }
}
