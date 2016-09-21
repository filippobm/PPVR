using System.Data.Entity;

namespace PPVR.WebApp.DataAccess.InitConfig
{
    public class ContextInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            context.SaveChanges();
        }

        private void ImportarCandidatos()
        {

        }
    }
}