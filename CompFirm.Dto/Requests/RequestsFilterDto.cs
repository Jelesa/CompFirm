namespace CompFirm.Dto.Requests
{
    public class RequestsFilterDto
    {
        public string SearchString { get; set; }

        public int? Status { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public bool AdminSearch { get; set; }
    }
}
