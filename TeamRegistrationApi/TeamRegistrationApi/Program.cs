    using LeaderboardAPI.Models;
    using Microsoft.EntityFrameworkCore;
    using TeamRegistrationApi.Data;

    var builder = WebApplication.CreateBuilder(args);

    // Registracija DbContext sa SQLite
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=teams.db"));


// Dodaj kontrolere i JSON reference handling
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Primjena migracija
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }

    // Middleware
    app.UseCors("AllowAll");
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
