using System.Data.Entity;

namespace CourseWork.UserClass
{
    class UserContext:DbContext
    {
        public UserContext() : base("UserConnection")
        {

        }
        public DbSet<User> Users { get; set; } 
    }
}
