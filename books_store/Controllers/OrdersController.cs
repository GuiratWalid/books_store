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
using static NuGet.Packaging.PackagingConstants;
using System.Net;

namespace books_store.Controllers
{
    public class OrdersController : Controller
    {
        private readonly books_storeContext _context;

        public OrdersController(books_storeContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
              return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create(int? id)
        {
            var book = await _context.Book.FindAsync(id);

            return View(book);
        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, int quantity)
        {
            Order order = new Order();
            order.bookId = bookId;
            order.quantity = quantity;

            Console.WriteLine(order);
            Console.WriteLine(order.quantity.ToString());

            order.userid = Convert.ToInt32(HttpContext.Session.GetString("userid")); ;
            order.orderdate = DateTime.Today;

            Console.WriteLine(order.orderdate);
            _context.Add(order);
            await _context.SaveChangesAsync();

            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Walid\\Documents\\books_store.mdf;Integrated Security=True;Connect Timeout=30");
            string sql;
            sql = "UPDATE book  SET bookquantity  = bookquantity   - '" + order.quantity + "'  where (id ='" + order.bookId + "' )";
            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();
            comm.ExecuteNonQuery();


            return RedirectToAction(nameof(MyOrders));

        }

        public async Task<IActionResult> MyOrders()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("userid")); ;
            var orItems = await _context.Order.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
            return View(orItems);
        }

        public async Task<IActionResult> CustomerOrders(int? id)
        {
            var orItems = await _context.Order.FromSqlRaw("select *  from orders where  userid = '" + id + "'  ").ToListAsync();
            return View(orItems);
        }


        public async Task<IActionResult> CustomerReport()
        {
            var orItems = await _context.Report.FromSqlRaw("select usersaccounts.id as Id, name as customername, sum (quantity * Price)  as total from book, orders,usersaccounts  where usersaccounts.id = orders.userid  and bookid= book.Id group by name,usersaccounts.id ").ToListAsync();
            return View(orItems);
        }



        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,bookId,userid,quantity,orderdate")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'books_storeContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Order.Any(e => e.Id == id);
        }
    }
}
