using System.ComponentModel.DataAnnotations;

namespace CourseWork.TextClass
{
    public class Text
    {
        public string UserName { get; set; }
        [Key]
        public string TextName { get; set; }
        public string TextWords { get; set; }
        public string TextLanguage { get; set; }
    }
}
