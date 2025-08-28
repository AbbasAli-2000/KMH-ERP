using FluentValidation;
using FluentValidation.AspNetCore;
using KMH_ERP.Application.Interfaces.Repository;
using KMH_ERP.Application.Validators;
using KMH_ERP.Infrastructure.Data;
using KMH_ERP.Infrastructure.Extensions;
using KMH_ERP.Infrastructure.Utilities.Halpers;
using KMH_ERP.Infrastructure.Utilities.Helpers;
using KMH_ERP.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (EF Core + Dapper + UnitOfWork + Repos)
builder.Services.AddInfrastructureServices(builder.Configuration);



// Session Configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true; 
});


builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RoleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RoleViewModelValidator>();




JwtHelper.setConfiguration(builder.Configuration);


// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();
var app = builder.Build();


using (var scope = app.Services.CreateAsyncScope())
{
    var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
    await DbInitializer.SeedAsync(unitOfWork);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseMiddleware<AuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


SessionHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
CookieHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
