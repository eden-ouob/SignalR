# SignalRController

使用 ASP.NET Core 框架所撰寫的一個 **MVC 控制器（Controller）**，建立一個回應網頁請求的端點。

```csharp
using Microsoft.AspNetCore.Mvc;

namespace otoSignalR.Controllers{
    public class SignalRController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
```
