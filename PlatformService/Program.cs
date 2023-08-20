using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;
using PlatformServices.AsyncDataServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

if(builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using Sql Server");
    builder.Services.AddDbContext<AppDbContext>(opt => 
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn"));
        opt.LogTo(Console.WriteLine);
    }
    );
}else
{
    Console.WriteLine("--> Using InMemory Database");
    builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, CommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();//Add Grpc Service

app.MapGet("/protos/platform.proto", async context => 
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platform.proto"));
});

PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
