var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHealthChecks();

var app = builder.Build();

app.UseHealthChecks("/diagnostic")
   .UseSwagger()
   .UseSwaggerUI()
   .UseHttpsRedirection();

app.MapControllers();

app.Run();
