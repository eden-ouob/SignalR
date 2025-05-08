---
description: with Javascript
---

# SignalR Frontend View (Javascript)

主要功能：

透過 **SignalR** 與 Server 端上的 `/noiseHub` 建立即時雙向連線，定期請求並接收 Noise data，並將接收到的Noise data 顯示在網頁上的指定區域。

***

程式分析：

* 設定 SignalR Hub 連線 `connection_noise`：除了以 `signalR.HubConnectionBuilder`建立與 Server 端hub的連線，其路由指定為伺服器的`/noiseHub` hub路徑，並根據所設定的參數（如 URL、協定、傳輸方式等），建立出一個 `HubConnection` 實體。

```javascript
const connection_noise = new signalR.HubConnectionBuilder()
                        .withUrl("/noiseHub")  
                        .build();
```



* 註冊**事件監聽器，**&#x7576; Server 端藉由 `connection_noise` 的 SignalR Hub 連線發出`"ReceiveNoiseData"` 的事件時，接收處理從 SignalR Hub 傳來的訊息 `noiseData`，然後把資料顯示在`"noiseData"`的區域。

```javascript
connection_noise.on("ReceiveNoiseData", function (noiseData) {
        document.getElementById("noiseData").innerText = "噪音：" + noiseData + 'dB';
});
```



* 開始 SignalR 的連接：`.then(function() {})`表示當連接成功建立時，會執行這個回呼函數。

```javascript
connection_noise.start().then(function() {
    console.log("SignalR_noise 連接已經啟動！");
```



* 每500毫秒執行一次回呼函數，也就是發出請求：判斷`connection_noise`的狀態為`Connected`後，\
  以`connection_noise.invoke("SendNoiseData")`向`connection_noise`發出`"SendNoiseData"`的請求。

```javascript
    setInterval(function() {
        if (connection_noise.state === signalR.HubConnectionState.Connected) {
            connection_noise.invoke("SendNoiseData").catch(function (err) {
                return console.error(err.toString());
            });
        } else {
            console.log("SignalR_noise 尚未連接，無法發送請求");
        }
    }, 500);

```

***

範例：Views/SignalR/Index.cshtml

```javascript
@{
    Layout = null;  // 這樣就不使用任何 Layout，這會移除 header 和 footer 部分
}

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>SignalR</title>
    <script src="/js/signalr.min.js"></script>
    <style>
        /* 顯示噪音數據區域 */
        .noise-data {
            margin-top: 20px;
            font-size: 18px;
            color: #333;
            font-weight: bold; /* 這裡確保字體為粗體 */
        }
    </style>
</head>
<body>
    <!-- 顯示噪音數據的區域 -->
    <div class="noise-data" id="noiseData">
        <div>噪音：等待偵測結果...</div>
    </div>

    <script>                

        // 設定 SignalR 連線
        const connection_noise = new signalR.HubConnectionBuilder()
            .withUrl("/noiseHub")  // 設定 SignalR noiseHub 的 URL
            .build();

        // 當接收到噪音數據時，更新顯示
        connection_noise.on("ReceiveNoiseData", function (noiseData) {
            document.getElementById("noiseData").innerText = "噪音：" + noiseData + 'dB';
        });

        // 開始連接 SignalR: noise
        connection_noise.start().then(function() {
            console.log("SignalR_noise 連接已經啟動！");

            // 只有在連接成功後才開始每秒請求時間更新
            setInterval(function() {
                if (connection_noise.state === signalR.HubConnectionState.Connected) {
                    connection_noise.invoke("SendNoiseData").catch(function (err) {
                        return console.error(err.toString());
                    });
                } else {
                    console.log("SignalR_noise 尚未連接，無法發送請求");
                }
            }, 500);

        }).catch(function(err) {
            console.error("SignalR_noise 連接錯誤: " + err);
        });

    </script>
</body>
</html>

```
