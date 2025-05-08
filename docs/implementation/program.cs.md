# Program.cs

加入 SignalR 服務： `builder.Services.AddSignalR();`

設定 SignalR Hub：`app.MapHub<NoiseHub>("/noisehub");`

設定網頁路由：

```csharp
app.MapControllerRoute(
name: "default",
pattern: "{controller=SignalR}/{action=Index}/{id?}");
```

***

整體程式碼：

```csharp
using otoSignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<NoiseHub>("/noisehub");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SignalR}/{action=Index}/{id?}");

app.Run();

```
