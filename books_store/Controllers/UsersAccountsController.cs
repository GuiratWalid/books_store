using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using books_store.Data;
using books_store.Models;
using Microsoft.Data.SqlClient;

namespace books_store.Controllers
{
    public class UsersAccountsController : Controller
    {
        private readonly books_storeContext _context;

        public UsersAccountsController(books_storeContext context)
        {
            _context = context;
        }

        // GET: UsersAccounts
        public async Task<IActionResult> Index()
        {
              return View(await _context.UsersAccounts.ToListAsync());
        }

        // GET: UsersAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsersAccounts == null)
            {
                return NotFound();
            }

            var usersAccounts = await _context.UsersAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersAccounts == null)
            {
                return NotFound();
            }

            return View(usersAccounts);
        }

        // GET: UsersAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string na, string pa)
        {
            SqlConnection conn1 = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Walid\\Documents\\books_store.mdf;Integrated Security=True;Connect Timeout=30");
            string sql;
            sql = "SELECT * FROM usersaccounts where name ='" + na + "' and  pass ='" + pa + "' ";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())
            {
                string role = (string)reader["role"];
                string id = Convert.ToString((int)reader["Id"]);
                HttpContext.Session.SetString("Name", na);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("userid", id);
                reader.Close();
                conn1.Close();
                if (role == "customer")
                    return RedirectToAction("catalogue", "books");

                else
                    return RedirectToAction("Index", "books");

            }
            else
            {
                ViewData["Message"] = "wrong user name password";
                return View();
            }
        }


        // POST: UsersAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,pass,email")] UsersAccounts usersAccounts)
        {
            //if (ModelState.IsValid)
            //{
                usersAccounts.role = "customer";
                _context.Add(usersAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            //}
            //return View(usersAccounts);
        }

        // GET: UsersAccounts/Edit/5
        public async Task<IActionResult> Edit()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("userid"));
            if (id == null || _context.UsersAccounts == null)
            {
                return NotFound();
            }

            var usersAccounts = await _context.UsersAccounts.FindAsync(id);
            if (usersAccounts == null)
            {
                return NotFound();
            }
            return View(usersAccounts);
        }

        // POST: UsersAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,pass,email")] UsersAccounts usersAccounts)
        {
            if (id != usersAccounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersAccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersAccountsExists(usersAccounts.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Login));
            }
            return View(usersAccounts);
        }

        // GET: UsersAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsersAccounts == null)
            {
                return NotFound();
            }

            var usersAccounts = await _context.UsersAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersAccounts == null)
            {
                return NotFound();
            }

            return View(usersAccounts);
        }

        // POST: UsersAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsersAccounts == null)
            {
                return Problem("Entity set 'books_storeContext.UsersAccounts'  is null.");
            }
            var usersAccounts = await _context.UsersAccounts.FindAsync(id);
            if (usersAccounts != null)
            {
                _context.UsersAccounts.Remove(usersAccounts);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersAccountsExists(int id)
        {
          return _context.UsersAccounts.Any(e => e.Id == id);
        }
    }
}
