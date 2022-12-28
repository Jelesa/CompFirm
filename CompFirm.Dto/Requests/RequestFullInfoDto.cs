using System.Collections.Generic;
using CompFirm.Dto.RequestItems;

namespace CompFirm.Dto.Requests
{
    public class RequestFullInfoDto : RequestShortInfoDto
    {
        public IReadOnlyCollection<RequestItemDto> RequestItems { get; set; }

        public IReadOnlyCollection<RequestJournalItemDto> Journal { get; set; }
    }
}
