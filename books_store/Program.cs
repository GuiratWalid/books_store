using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using books_store.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<books_storeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("books_storeContext") ?? throw new InvalidOperationException("Connection string 'books_storeContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(1); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UsersAccounts}/{action=Login}/{id?}");

app.Run();
