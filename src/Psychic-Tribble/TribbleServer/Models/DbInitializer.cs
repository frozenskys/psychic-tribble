using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TribbleServer.Models
{
    public static class DbInitializer
    {
        public static void Initialize(TribbleContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Tribbles.Any())
            {
                return;   // DB has been seeded
            }

            var tribbles = new Tribble[]
            {
                new Tribble{Id = 1, Colour = "blue", Furryness = "High", Hungry = true},
                new Tribble{Id = 2, Colour = "red", Furryness = "Low", Hungry = true}
            };
            foreach (Tribble t in tribbles)
            {
                context.Tribbles.Add(t);
            }
            context.SaveChanges();
        }
    }
}
