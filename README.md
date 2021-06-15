# ChinaAQIDataCore
🇨🇳中国城市AQI数据(An other AQI Data of Cities in China) Written with C# on .NET 5 platform.

另外一个项目 [pm25](https://github.com/ettingshausen/pm25)  调用了 pm25.in 接口，但是最近 pm25.in 网站非常的不稳定，于是萌生了自己写后端的想法。

![ScreenShot1](http://ww2.sinaimg.cn/bmiddle/685ea4fagw1eyymnnzflij20u01hc14k.jpg)
![ScreenShot2](http://ww1.sinaimg.cn/bmiddle/685ea4fagw1eyymnpw4vdj20u01hc45y.jpg)

实现参考了[ChinaAQIData](https://github.com/geoinsights/ChinaAQIData)， 做了些改进，抓取数据与接口服务放在了一起，每半个小时会抓取一次数据。



# Thanks to

1. http://www.pm25.in
1. https://openaq.org/
1. https://github.com/geoinsights/ChinaAQIData
