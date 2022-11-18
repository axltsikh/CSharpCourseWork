using System.ComponentModel.DataAnnotations;

namespace CourseWork.UserClass
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserCountry { get; set; }
        public byte[] AvatarImage { get; set; }
    }
}
