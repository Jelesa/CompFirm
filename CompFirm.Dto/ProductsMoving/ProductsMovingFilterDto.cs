using CompFirm.Dto.SearchFilter;

namespace CompFirm.Dto.ProductsMoving
{
    public class ProductsMovingFilterDto : BaseSearchFilterDto
    {
        public string MovingType { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }
    }
}
