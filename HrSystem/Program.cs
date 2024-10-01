using Microsoft.EntityFrameworkCore;
using DataLayer.Data;
using Microsoft.AspNetCore.Identity;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Entities;
using BusinessLogic.Services;
using HrSystem.Services;
using DataLayer.Settings;
using BusinessLayer.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
    .AddViewOptions(option =>
    {
        option.HtmlHelperOptions.ClientValidationEnabled = true;
    });

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login"; // Set your custom login path


    options.AccessDeniedPath = "/Errors/NowAllowd"; // Access denied page
    
});

//abdallah11110000
//A951M951

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure Mail Settings
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Register Services
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IPrivateHolidayService, PrivateHolidayService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDepartmentsService, DepartmentsService>();

// Register SuperAdminSeeder
builder.Services.AddScoped<SuperAdminSeeder>();

builder.Services.AddAuthorization(options =>
{
    foreach (var claim in Claims.AllClaims)
    {
        options.AddPolicy(claim.Type, policy =>
            policy.RequireClaim(claim.Type));
    }
});

var app = builder.Build();

// Apply Seed Data in Development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<SuperAdminSeeder>();
        await seeder.SeedAsync();
    }
}

// Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication should be before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
