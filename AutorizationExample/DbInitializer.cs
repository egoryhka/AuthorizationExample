using AutorizationExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample
{
    public class DbInitializer
    {
        public static void Initialize(ProjectActivityDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
            new User{Login="Egor", Password="Demyanenko", Admin=true },
            new User{Login="Mrsh",Password="Kid",Admin=false},
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

        }
    }
}
