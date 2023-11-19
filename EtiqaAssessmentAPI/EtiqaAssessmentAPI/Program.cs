using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = String.Empty;
//if (builder.Environment.IsDevelopment())
//{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
//}
//else
//{
//    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
//}

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connection, providerOptions => providerOptions.EnableRetryOnFailure()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/User", (UserDbContext context) =>
{
    return context.User.ToList();
})
.WithName("Get")
.WithOpenApi();

app.MapPost("/User", (User person, UserDbContext context) =>
{
    context.Add(person);
    context.SaveChanges();
})
.WithName("Create")
.WithOpenApi();

app.MapPut("/User", (User person, UserDbContext context) =>
{
    context.Update(person);
    context.SaveChanges();
})
.WithName("Update")
.WithOpenApi();

app.MapDelete("/User", (int id, UserDbContext context) =>
{
    var user = context.Find<User>(id);
    //person.Deleted = true;
    //context.Update(person);
    context.Remove(user);
    context.SaveChanges();
})
.WithName("Delete")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Skillsets { get; set; }
    public string Hobbies { get; set; }
    public string Status { get; set; }
    public bool Deleted { get; set; }
}

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; }
}

//public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
//{
//    public UserDbContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
//        optionsBuilder.UseSqlServer(connection);

//        return new UserDbContext(optionsBuilder.Options);
//    }
//}

//public class Skillset
//{
//    public int Id { get; set; }
//    public int UserId { get; set; }
//    public string Skill { get; set; }
//}

//public class Hobbies
//{
//    public int Id { get; set; }
//    public int UserId { get; set; }
//    public string Hobby { get; set; }
//}

//public class SkillsetDbContext : DbContext
//{
//    public SkillsetDbContext(DbContextOptions<SkillsetDbContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<Skillset> Skills { get; set; }
//}

//public class HobbyDbContext : DbContext
//{
//    public HobbyDbContext(DbContextOptions<HobbyDbContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<Hobbies> Hobbies { get; set; }
//}