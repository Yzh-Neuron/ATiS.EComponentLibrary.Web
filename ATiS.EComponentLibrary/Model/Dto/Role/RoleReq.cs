using System.ComponentModel;

namespace Model.Dto.Role
{
    public class RoleReq
    {
        public string Name { get; set; }
        [DefaultValue("Hello")]
        public string Description { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
