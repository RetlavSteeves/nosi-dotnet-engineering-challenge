using Microsoft.EntityFrameworkCore;
using NOS.Engineering.Challenge.API.Extensions;
using NOS.Engineering.Challenge.Database;


var builder = WebApplication.CreateBuilder(args)
        .ConfigureWebHost()
        .RegisterServices();

    // builder.Services.AddDbContext<DataContext>(options =>{
    //         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //     });

var app = builder.Build();

app.UseOutputCache();

app.MapControllers();
app.UseSwagger()
    .UseSwaggerUI();
    
app.Run();