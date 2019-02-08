using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlazorCore.Infrastructure.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            //TODO: remove code for migrations
            context.Database.EnsureCreated();
        }
    }
}