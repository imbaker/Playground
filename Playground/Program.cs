using Microsoft.AspNetCore.Builder;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog();

var app = builder.Build();


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");