using Books.BL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(op => {
    op.AddPolicy("AllowAll", policy => {
        policy.WithOrigins("http://localhost:3000/");
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddDbContext<BooksContext>(op => op
    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")), 
        b => b.MigrationsAssembly("Books.API"))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();