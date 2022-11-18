using System.Data.Entity;


namespace CourseWork.TextClass
{
    class TextContext:DbContext
    {
        public TextContext() : base("UserConnection")
        {

        }
        public DbSet<Text> Texts { get; set; }
    }
}
