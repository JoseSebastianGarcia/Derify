using Derify.Core;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDerify("Server=NTTD-5C8NNQ3\\NTTDATA;Database=dbtest;User Id=sa;Password=Indiana2028.-;TrustServerCertificate=True");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDerify();

app.MapControllers();

app.Run();
