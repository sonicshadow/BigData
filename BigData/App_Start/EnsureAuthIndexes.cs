using AspNet.Identity.MongoDB;

namespace BigData
{


    public class EnsureAuthIndexes
    {
        public static void Exist()
        {
            var context = new ApplicationDbContext();
            IndexChecks.EnsureUniqueIndexOnUserName(context.IdentityUsers);
            IndexChecks.EnsureUniqueIndexOnRoleName(context.Roles);
        }
    }
}