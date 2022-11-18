using System.ComponentModel.DataAnnotations;


namespace CourseWork.GameClass
{
    public class Game
    {
        [Key]
        public int GameID { get; set; }
        public string UserName { get; set; }
        public string TextName { get; set; }
        public string TextLanguage { get; set; }
        public int Result { get; set; }
        public Game(string _UserName,string _TextName,string _TextLanguage,int _Result)
        {
            UserName = _UserName;
            TextName = _TextName;
            TextLanguage = _TextLanguage;
            Result = _Result;
        }
        public Game()
        {

        }
    }
}
