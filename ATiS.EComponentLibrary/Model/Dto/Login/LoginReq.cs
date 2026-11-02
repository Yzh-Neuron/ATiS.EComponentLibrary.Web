using System.ComponentModel.DataAnnotations;
namespace Model.Dto.Login
{
    public class LoginReq
    {
        [Required(ErrorMessage ="自定义的错误信息1")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "自定义的错误信息2")]
        public string PassWord { get; set; }
    }
}