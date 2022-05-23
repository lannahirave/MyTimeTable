using Microsoft.EntityFrameworkCore;
using MyTimeTable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MyTimeTableContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddControllersWithViews();
var app = builder.Build();
// using (var scope = app.Services.CreateScope())
// {
//     await scope.ServiceProvider.GetRequiredService<MyTimeTableContext>().Database.MigrateAsync();
// }

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


app.MapRazorPages();

app.Run();