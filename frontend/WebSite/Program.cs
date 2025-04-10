using Website.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("CatalogApi", client =>
{
    var catalogoServiceUrl = builder.Configuration["Services:Catalogo"] ?? "http://localhost:5211";
    client.BaseAddress = new Uri($"{catalogoServiceUrl}/api/");
});
builder.Services.AddHttpClient("PedidosApi", client =>
{
    var orderServiceUrl = builder.Configuration["Services:Pedidos"] ?? "http://localhost:5272";
    client.BaseAddress = new Uri($"{orderServiceUrl}/api/");
});

builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
