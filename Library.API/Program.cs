using Library.API.Extensions;
using Library.Application.Interfaces.Auth;
using Library.Application.Services;
using Library.Core.Abstaction;
using Library.Core.Abstraction;
using Library.Core.Services;
using Library.DataAccess;
using Library.DataAccess.Repositories;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(LibraryDbContext)));
    }
    );

builder.Services.AddApiAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

builder.Services.AddScoped<ILibraryService, LibrariesService>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();

builder.Services.AddScoped<ILibrarianService, LibrariansService>();
builder.Services.AddScoped<ILibrarianRepository, LibrarianRepository>();

builder.Services.AddScoped<IReadingRoomService, ReadingRoomService>();
builder.Services.AddScoped<IReadingRoomRepository, ReadingRoomRepository>();

builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();

builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IReaderRepository, ReaderRepository>();

builder.Services.AddScoped<IReaderCategoryService, ReaderCategoryService>();
builder.Services.AddScoped<IReaderCategoryRepository, ReaderCategoryRepository>();

builder.Services.AddScoped<IItemCategoryService, ItemCategoryService>();
builder.Services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddScoped<IItemCopyService, ItemCopyService>();
builder.Services.AddScoped<IItemCopyRepository, ItemCopyRepository>();

builder.Services.AddScoped<IShelfService, ShelfService>();
builder.Services.AddScoped<IShelfRepository, ShelfRepository>();

builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapGet("get", () =>
//{
//    return Results.Ok('ok');
//});


app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

app.Run();
