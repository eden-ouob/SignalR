# NoiseHub

Hubs/Noisehub.cs：於 Hubs 資料夾建立新類別 NoiseHub

嘗試讀取檔案：

```csharp
string noiseData = File.ReadAllText(filePath);
```

送出數據至 Client 端：

```csharp
await Clients.All.SendAsync("ReceiveNoisedata", noiseData);
```

***

整體程式碼 (含錯誤日誌寫入)：

{% code lineNumbers="true" %}
```csharp
using Microsoft.AspNetCore.SignalR;

namespace otoSignalR.Hubs
{
    public class NoiseHub : Hub
    {
        public async Task SendNoiseData()
        {
            // 檔案路徑
            var filePath = Path.Combine(AppContext.BaseDirectory, "noise.txt");
            // 確保檔案存在
            if (File.Exists(filePath))
            {
                try
                {
                    // 嘗試讀取檔案
                    string noiseData = File.ReadAllText(filePath);
                    // 送出數據至 Client 端
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

                // 記錄檔案不存在的日誌
                WriteLog($"檔案 {filePath} 不存在");
            }
        }

        // 將日誌訊息寫入檔案
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
{% endcode %}
