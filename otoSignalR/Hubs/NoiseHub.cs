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