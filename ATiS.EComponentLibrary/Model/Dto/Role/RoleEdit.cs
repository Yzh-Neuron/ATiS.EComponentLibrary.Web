namespace Model.Dto.Role
{
    public class RoleEdit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsEnable { get; set; }
        public string Description { get; set; }
    }
}
