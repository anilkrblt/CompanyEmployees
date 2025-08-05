using CompanyEmployees.Extensions;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureMySqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // api controllerdeki varsayılan model state kontrolünü devre dışı bırak
});

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services.BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();

// add services to container
builder
    .Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true; // Accept headerin değerini umursar hale getirir böylece xml yanıt dönebiliriz.
        config.ReturnHttpNotAcceptable = true; // Bilinmeyen Accept değerleri için 406 Not Acceptable hatası döner
        config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    })
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCSVFormatter()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

// hizmeti yukarıdaki satırdan sonra çıkarmak önemli çünkü build metodu
// WebApplication u oluşturur ve eklenen tüm hizmetleri IOC ile kaydeder
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
    ;
// app.UseDeveloperExceptionPage();
else
    // app.UseHsts() will add middleware for using HSTS, which adds the
    // Strict-Transport-Security header.
    app.UseHsts();

// The UseHttpRedirection method is used to add the middleware for the
// redirection from HTTP to HTTPS.
app.UseHttpsRedirection();

// app.UseStaticFiles() enables using static files for the request.
// If  we don’t set a path to the static files directory, it will use a wwwroot
// folder in our project by default.
app.UseStaticFiles();

// app.UseForwardedHeaders() mevcut isteğe proxy başlıklarını iletecektir
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

app.UseCors("CorsPolicy");

// Also, we can see the UseAuthorization
// method that adds the authorization middleware to the specified
// IApplicationBuilder to enable authorization capabilities.
app.UseAuthorization();

/*
app.Use(async (context, next) =>
{
Console.WriteLine($"Logic before executing the next delegate in the Use method");
await next.Invoke();
Console.WriteLine($"Logic after executing the next delegate in the Use method");
});
app.Run(async context =>
{
Console.WriteLine($"Writing the response to the client in the Run method");
await context.Response.WriteAsync("Hello from the middleware component.");
});
*/

app.MapControllers();

/*
app.MapAreaControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.UseRouting();
*/

app.Run();

// http istek sırası
// Exception Handler -> HSTS -> HttpsRedirection -> Static Files -> Routing -> CORS -> Authentication -> Authorization -> custom middleware
// app.MapGet("/", () => "Hello World!"); böyle custom pipeline oluşturabiliriz
// terminal metod
/*
app.Run(async context =>
{
await context.Response.WriteAsync("Hello from the middleware component.");
});
*/
/* Map metodundan sonra eklenen herhangi bir middleware çalışmaz
app.Map(
    "/usingmapbranch",
    builder =>
    {
        builder.Use(
            async (context, next) =>
            {
                Console.WriteLine("map branch logic in the use method before next delegate");
                await next.Invoke();
                Console.WriteLine("map branch logic in the use method after next delegate");
            }
        );
        builder.Run(async context =>
        {
            Console.WriteLine("writing the rosponse to the client in the run method");
            await context.Response.WriteAsync("Hello from the middleware component");
        });
    }
);
*/

/*
    dotnet IOC conteynerine üç şekilde ekleme yapılır:
    services.AddSingleon => bir kere servisi oluşturur ve tüm istekler aynı servisi kullanır

    services.AddScoped => bir HTTP isteği gönderdiğimizde hizmetin yeni bir örneği oluşturulur

    services.AddTransient => uygulamaya her istek geldiğinde yeniden yaratılır, birden fazla bileşenin
    bu hizmete ihtiyacı varsa her bir bileşen talebi için hizmetin yeniden oluşturulacağı anlamına gelir

*/
