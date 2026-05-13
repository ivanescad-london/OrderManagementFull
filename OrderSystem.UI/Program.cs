using OrderSystem.UI.Components;
using OrderSystem.UI.Models;
using OrderSystem.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Call Backend API from Blazor
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7296/") // backend URL
});

// Register Services
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<GoodService>();
builder.Services.AddScoped<OrderCreateDtoService>();
builder.Services.AddScoped<OrderReadDtoService>();
builder.Services.AddScoped<OrderUpdateDtoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
