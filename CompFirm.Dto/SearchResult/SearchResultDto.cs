using System.Collections.Generic;

namespace CompFirm.Dto.SearchResult
{
    public class SearchResultDto<T>
    {
        public int Found { get; set; }

        public IReadOnlyCollection<T> Result { get; set; }
    }
}
