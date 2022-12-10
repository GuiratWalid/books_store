using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using books_store.Models;

namespace books_store.Data
{
    public class books_storeContext : DbContext
    {
        public books_storeContext (DbContextOptions<books_storeContext> options)
            : base(options)
        {
        }

        public DbSet<books_store.Models.Book> Book { get; set; } = default!;
    }
}
