using System.Runtime.Serialization;

namespace CompFirm.Dto
{
    [DataContract]
    public class ExceptionResponseDto
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
