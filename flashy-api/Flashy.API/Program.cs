using System.Text;
using Flashy.API.Infrastructure.Data;
using Flashy.API.Infrastructure.Data.Migrations;
using Flashy.API.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
  var builder = WebApplication.CreateBuilder(args);

  builder.Host.UseSerilog((context, configuration) => configuration
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(context.Configuration));

  builder.Services.AddDatabaseDeveloperPageExceptionFilter();
  builder.Services.AddDbContextPool<FlashyAPIDBContext>(options =>
  {
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
      throw new InvalidOperationException("Connection string 'FlasyConnection' not found.");

    options.UseSqlServer(connectionString, builder =>
    {
      // Migrations
      builder.MigrationsAssembly("Flashy.API.Infrastructure");
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
  builder.Services.AddSingleton<AuthService, AuthService>();

  builder.Services.AddIdentity<User, Role>(options =>
  {
    // User Options
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

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
    .AddEntityFrameworkStores<FlashyAPIDBContext>()
    .AddDefaultTokenProviders();

  builder.Services.AddAuthentication(options =>
  {
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

    options.RequireAuthenticatedSignIn = true;
  })
    .AddJwtBearer(options =>
    {
      var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ??
        throw new InvalidOperationException("JWT Key not found."));
      var issuer = builder.Configuration["JWT:Issuer"] ??
        throw new InvalidOperationException("JWT Issuer not found.");
      var audience = builder.Configuration["JWT:Audience"] ??
        throw new InvalidOperationException("JWT Issuer not found.");

      options.TokenValidationParameters.ValidateIssuer = true;
      options.TokenValidationParameters.ValidateAudience = true;
      options.TokenValidationParameters.ValidateLifetime = true;
      options.TokenValidationParameters.ValidateIssuerSigningKey = true;
      options.TokenValidationParameters.ValidIssuer = issuer;
      options.TokenValidationParameters.ValidAudience = audience;
      options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);

      options.SaveToken = true;

      // TODO: Change to TRUE
      options.RequireHttpsMetadata = false;
    });

  builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

  builder.Services.AddControllers();

  // Add services to the container.
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(setupAction: options =>
  {
    var securityScheme = new OpenApiSecurityScheme
    {
      Name = "JWT Authentication",
      Description = "Enter JWT Token",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      Scheme = "bearer",
      BearerFormat = "JWT"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        },
        Array.Empty<string>()
      }
    };

    options.AddSecurityRequirement(securityRequirement);
  });


  var app = builder.Build();

  // Seed initial data
  using (var scope = app.Services.CreateScope())
  {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<FlashyAPIDBContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();

    context.Database.Migrate();
    SeedData.InitializeAdmin(userManager, roleManager).Wait();
    SeedData.InitializeUser(userManager, roleManager).Wait();
  }

  app.UseSerilogRequestLogging();

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
    app.UseHsts();
  }

  app.UseHttpsRedirection();

  app.UseRouting();

  app.UseAuthentication();
  app.UseAuthorization();

  app.MapControllers();

  app.Run();
}
catch (Exception e)
{
  Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
  Log.Information("Shutdown completed");
  Log.CloseAndFlush();
}
