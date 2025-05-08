# Noise data simulation

以簡單的亂數產生器模擬噪音文件數值變化

```python
import random
import time

while True:
    number = random.randint(0, 99)
    with open("noise.txt", "w") as f:  # 使用 'w' 模式會覆蓋舊資料
        f.write(str(number))
    print(f"更新數字: {number}")
    time.sleep(1)  # 每秒更新一次
```

網頁中即可顯示噪音文件內的數值

<figure><img src="../.gitbook/assets/1746520282639 (1).gif" alt=""><figcaption></figcaption></figure>
