using BankAccountAPI.Middlewares;
using BankAccountAPI.Data;
using BankAccountAPI.Filters;
using BankAccountAPI.Services.Implementation;
using BankAccountAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//builder.Services.AddControllers(options => options.Filters.Add<ValdationFilter>());
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("BankAccountDB")));

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
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<ITransactionService, TransactionService>();
}