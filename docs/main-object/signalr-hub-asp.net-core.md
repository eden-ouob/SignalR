---
description: with ASP.Net Core MVC
---

# SignalR Hub (ASP.Net Core)

主要功能：

透過 ASP.NET Core SignalR 建立 <mark style="color:yellow;">**Server 端 Hub 類別**</mark>，Server 端讀取特定時間開頭命名的文字檔案，擷取其中一部分資料，即時推送給所有已連線的 Client 端。

***

程式分析：

* 命名空間：使用了 **SignalR**，一個 ASP.NET Core 的即時通訊框架。它讓 Server 端可以即時**推送**訊息給Client 端，而不是等 Client 端發出請求。

```csharp
using Microsoft.AspNetCore.SignalR;
```



* 定義`NoiseHub`：NoiseHub 繼承自 Hub，代表這是一個 SignalR 的 Hub。\
  Hub 就像一個**訊息中繼站**，管理 Server 端和 Client 端之間的即時溝通。

```csharp
public class NoiseHub : Hub
```



* 定義`SendNoiseData()`：一種 Server 端的方法，找出特定命名規則的檔案後，從第一個找到的檔案中讀取資料，並傳送該資料給所有 Client 端。

```csharp
public async Task SendNoiseData()
```



* 建立檔名時間前綴：用**年月日時** (例如：2025050615) 的格式產生時間前綴。也就是**每小時**會檢查一次是否有以該時間開頭命名的 .txt 檔案。

```csharp
var timePrefix = DateTime.Now.ToString("yyyyMMddHH");
```



* 搜尋 folderPath 裡以`timePrefix`開頭的檔案：

```csharp
var files = Directory.GetFiles(folderPath, timePrefix + "*.txt");
```



* 找到正確資料後，讀取與處理其中的資料：檔案內容以逗號分隔，讀取第二個欄位資料（parts\[1]）。

```csharp
string fileContent = File.ReadAllText(filePath);
var parts = fileContent.Split(',');
var extractedAreaData = parts[1]; // 假設有至少兩個欄位
```



* 推送到 Client 端：將擷取到的資料透過 SignalR 傳給所有連線中的 Client 端。Client 端需實作對 `ReceiveNoisedata` 的接收與處理邏輯。

```csharp
await Clients.All.SendAsync("ReceiveNoisedata", extractedNoiseData);
```

***

範例：Hubs/Noisehub.cs

```csharp
using Microsoft.AspNetCore.SignalR;

namespace otoSignalR.Hubs
{
    public class NoiseHub : Hub
    {
        public async Task SendNoiseData()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "noise.txt");
            // 確保檔案存在
            if (File.Exists(filePath))
            {
                try
                {
                    string noiseData = File.ReadAllText(filePath);
                    await Clients.All.SendAsync("ReceiveNoisedata", noiseData);
                }
                catch (Exception ex)
                {
                    // 捕捉錯誤並記錄錯誤日誌
                    WriteLog($"讀取檔案錯誤: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"檔案 {filePath} 不存在");

                // 記錄檔案不存在的錯誤日誌
                WriteLog($"檔案 {filePath} 不存在");
            }
        }

        // 將錯誤日誌訊息寫入檔案
        private async void WriteLog(string message)
        {
            try
            {
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt");
                // 確保目錄存在，如果不存在則創建
                string directoryPath = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");

                //await _hubContext.Clients.All.SendAsync("ReceiveLog", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"寫入日誌錯誤: {ex.Message}");
            }
        }
    }
}
```
