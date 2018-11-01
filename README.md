# 如何加载该场景

1. 启动 Unity，在 Projects窗口中选择 Open，并在弹出窗口中选择刚才的项目目录 AR_Tooth并确认
2. 在菜单栏中，选择 Assets > Import Package > Custom Package，找到事先下载的 HoloToolkit-Unity包，在弹出的窗口中选择全部组件，并点击 Import导入
3. 在 Project窗格中，选择 Assets目录，并双击载入 Tooth场景
4. 在菜单栏中，选择 Edit > Project Settings > Quality，将 Levels的 Default设置为 Very Low；
5. 在菜单栏中，选择 Edit > Project Settings > Player，在面板中切换至 UWP Settings，展开 Other Settings选项卡，将 Scripting Backend设为 .NET，再展开 XR Settings选项卡，选中 Virtual Reality Supported；
6. 在菜单栏中，选择 File > Build Settings，选中 UWP后点击 Switch Platform，然后将 Target Device设为 Hololens，再选中 Unity C# Projects
7. 编译、部署、运行
