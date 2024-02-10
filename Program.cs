using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Flashy.Data;
using Flashy.Shared.Models;
using Flashy.Data.Context;
using Microsoft.AspNetCore.Identity;
using Flashy.Data.Repositories.Identity;
using Flashy.Data.Configurations.Identity;
using Flashy.Shared.Models.Identity;
using Flashy.Shared.Services.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
  // Change FlashyConnection to DefaultConnection for local SQL Express development
  var connectionString = builder.Configuration.GetConnectionString("FlashyConnection") ??
    throw new InvalidOperationException("Connection string 'FlasyConnection' not found.");

  options.UseSqlServer(connectionString, builder =>
  {
    // Migrations
    builder.MigrationsAssembly("Flashy.Data");
    builder.MigrationsHistoryTable("__EFMigrationsHistory");

    // Query Options
    builder.CommandTimeout(10);
    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
  });

  // Logging Options
  if (builder.Environment.IsDevelopment())
  {
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
    options.LogTo(Console.WriteLine, LogLevel.Information);
  }

  options.UseLazyLoadingProxies();
});


// Identity
builder.Services.AddScoped<RoleManager<Role>, ApplicationRoleManager>();
builder.Services.AddScoped<SignInManager<User>, ApplicationSignInManager>();
builder.Services.AddScoped<UserManager<User>, ApplicationUserManager>();
builder.Services.AddScoped<IRoleClaimStore<Role>, RoleStore>();
builder.Services.AddScoped<IRoleStore<Role>, RoleStore>();
builder.Services.AddScoped<IUserAuthenticationTokenStore<User>, UserStore>();
// builder.Services.AddScoped<IUserAuthenticatorKeyStore<User>, UserStore>();
builder.Services.AddScoped<IUserClaimStore<User>, UserStore>();
builder.Services.AddScoped<IUserEmailStore<User>, UserStore>();
builder.Services.AddScoped<IUserLockoutStore<User>, UserStore>();
builder.Services.AddScoped<IUserLoginStore<User>, UserStore>();
builder.Services.AddScoped<IUserPasswordStore<User>, UserStore>();
builder.Services.AddScoped<IUserPhoneNumberStore<User>, UserStore>();
builder.Services.AddScoped<IUserRoleStore<User>, UserRoleStore>();
builder.Services.AddScoped<IUserSecurityStampStore<User>, UserStore>();
builder.Services.AddScoped<IUserStore<User>, UserStore>();
builder.Services.AddScoped<IProtectedUserStore<User>, UserStore>();
// builder.Services.AddScoped<IUserTwoFactorRecoveryCodeStore<User>, UserTwoFactorRecoveryCodeStore>();
builder.Services.AddScoped<IUserTwoFactorStore<User>, UserStore>();

builder.Services.AddIdentity<User, Role>(options =>
{
  // User Options
  options.User.RequireUniqueEmail = true;
  options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

  // Lockout Options
  options.Lockout.AllowedForNewUsers = true;
  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
  options.Lockout.MaxFailedAccessAttempts = 5;

  options.Stores.ProtectPersonalData = false;

  // SignIn Options
  options.SignIn.RequireConfirmedAccount = true;
  options.SignIn.RequireConfirmedEmail = true;
  // options.SignIn.RequireConfirmedPhoneNumber = true;

  // Password Options
  options.Password.RequiredLength = 8;
  options.Password.RequiredUniqueChars = 6;
  options.Password.RequireDigit = true;
  options.Password.RequireLowercase = true;
  options.Password.RequireUppercase = true;
  options.Password.RequireNonAlphanumeric = true;
})
.AddDefaultUI()
.AddApiEndpoints()
.AddDefaultTokenProviders()
.AddUserStore<UserStore>()
.AddUserManager<ApplicationUserManager>()
.AddRoleStore<RoleStore>()
.AddRoleManager<ApplicationRoleManager>()
.AddSignInManager<ApplicationSignInManager>();

// builder.Services.AddAuthentication(options =>
// {
//   options.DefaultScheme = IdentityConstants.ApplicationScheme;
//   options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//   options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
//   options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//   options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//   options.DefaultForbidScheme = IdentityConstants.ApplicationScheme;

//   options.RequireAuthenticatedSignIn = true;
// });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

  // Swagger
  app.UseSwagger();
  app.UseSwaggerUI();

  // Migrations
  app.UseMigrationsEndPoint();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.MapRazorPages();

app.Run();
