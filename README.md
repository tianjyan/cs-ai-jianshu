一个[简书](http://www.jianshu.com/)的Universal Windows Platform的客户端，仅供学习和研究。

# 开发环境
Visual Studio 2015
Universal Windows SDK 10240
Windows 10 10240

# 如何使用
* 安装lib下的sqlite-uwp-3130000.vsix文件(sha1: eb3b6b66c8be1e9fab5f7d08fa90421a179bddcb)
* 打开AiJianShu.sln，选择你需要的运行平台(推荐x86)，运行即可。

# 使用说明
* 因为简书官方的API已经全部启用ssl了，这个版本的API还是旧版本的API，给简书官方发送的版本号为安卓版本号的1.9.1。如果官方不再支持1.9.1，这个版本的程序就不能运行了。
* 如果要使用新浪微博的分享功能，需要添加自己的Key的信息，文件在SocialShare项目的Config.cs文件中。


