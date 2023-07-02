using BlazorApp_empty1;
using ClassLibrary1;
using ClassLibrary1.Input;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ElementReferenceState>(new Func<IServiceProvider, ElementReferenceState>((x) => new ElementReferenceState()));
builder.Services.AddSingleton<ICanvasContextDrawer, CanvasContextDrawer>();
builder.Services.AddSingleton<IGameLooper, GameLooper>();
builder.Services.AddSingleton<Canvas2DContextState>(new Func<IServiceProvider, Canvas2DContextState>((x) => new Canvas2DContextState()));
builder.Services.AddSingleton<FPSState>(new Func<IServiceProvider, FPSState>((x) => new FPSState()));
builder.Services.AddSingleton<IKeyReporter, KeyReporter>();

await builder.Build().RunAsync();

