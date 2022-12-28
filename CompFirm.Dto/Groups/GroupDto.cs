namespace CompFirm.Dto.Groups
{
    public class GroupDto
    {
        public ulong Id { get; set; }
        public int? ParentGroupId { get; set; }
        public string ParentGroupName { get; set; }
        public string Name { get; set; }
    }
}
