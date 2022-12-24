using System.Text;
using Books.API;
using Books.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(op => {
    op.AddPolicy("AllowAll", policy => {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5000")
            .AllowAnyMethod()
            .WithHeaders(HeaderNames.ContentType, HeaderNames.SetCookie, HeaderNames.Cookie, HeaderNames.Authorization)
            .AllowCredentials();
    });
});

var jwtSettings = builder.Configuration.GetRequiredSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => {
        o.TokenValidationParameters = new TokenValidationParameters {
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        o.Events = new JwtBearerEvents {
            OnMessageReceived = context => {
                context.Token = context.Request.Cookies["access-token"];
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();


builder.Services.AddDbContext<BooksContext>(op => op
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")), 
        b => b.MigrationsAssembly("Books.API"))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());

builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();