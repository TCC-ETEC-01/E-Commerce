using ProjetoEcommerce.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Injetando dependências
builder.Services.AddScoped<ClienteRepositorio>();
builder.Services.AddScoped<FuncionarioRepositorio>();
builder.Services.AddScoped<PacoteRepositorio>();
builder.Services.AddScoped<ProdutoRepositorio>();
builder.Services.AddScoped<ViagemRepositorio>();
builder.Services.AddScoped<PassagemRepositorio>();
builder.Services.AddScoped<PassagemComViagemRepositorio>();
builder.Services.AddScoped<PacoteComPassagemProdutoRepositorio>();



builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=CadastroHome}/{id?}");
app.Run();
