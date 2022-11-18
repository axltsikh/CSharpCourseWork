using System.Data.Entity;


namespace CourseWork.GameClass
{
    class GameContext:DbContext
    {
        public GameContext() : base("UserConnection")
        {

        }
        public DbSet<Game> Games { get; set; }
    }
}
