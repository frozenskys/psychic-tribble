namespace TribbleServer.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using TribbleServer.Models;


    public class TribblesController : ApiController
    {
        public Tribble Get(int id)
        {
            Tribble tribble;
            
            using (var db = new TribbleContext())
            {
                DbInitializer.Initialize(db);
                tribble = db.Tribbles.FirstOrDefault(s => s.Id == id);
            }
            return tribble;
        }

        public Tribble[] Get()
        {
            Tribble[] tribbles;

            using (var db = new TribbleContext())
            {
                DbInitializer.Initialize(db);
                tribbles = db.Tribbles.ToArray();
            }
            return tribbles;
        }
    }
}
