using Microsoft.EntityFrameworkCore;
using DataLayer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors.Infrastructure;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Entities;
using BusinessLogic.Services;
using HrSystem.Services;
using DataLayer.Settings;
using BusinessLayer.Seeding;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

object value = builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
    .AddViewOptions(option =>
    {
        option.HtmlHelperOptions.ClientValidationEnabled = true;
    });

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
//HrSystem
//cusp qlek xkjx hvdn


builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IPrivateHolidayService, PrivateHolidayService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<SuperAdminSeeder>();





builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{

    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


//allow easy password


//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();


var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<SuperAdminSeeder>();
    await seeder.SeedAsync();
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
