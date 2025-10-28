# SlimTrack

一个基于 .NET 9 和 Blazor WebAssembly 的轻量级体重追踪应用。

## ?? 项目简介

SlimTrack 是一个简洁易用的体重记录和管理工具，帮助用户追踪体重变化趋势。该应用采用前后端分离架构，提供直观的数据可视化和便捷的数据管理功能。

## ? 主要功能

- ? 体重记录的添加、更新和删除
- ? 基于日期的数据可视化图表
- ? 体重变化趋势统计
- ? 支持日期范围查询
- ? 每日备注记录
- ? 自动数据聚合和统计分析

## ??? 技术栈

### 前端
- **Blazor WebAssembly** - 基于 .NET 9 的现代化 Web 框架
- **C# 13.0** - 最新的 C# 语言特性
- **Chart.js** - 数据可视化（通过 WeightChart 组件）

### 后端
- **ASP.NET Core 9** - Web API 服务
- **Entity Framework Core** - ORM 框架
- **SQLite** - 轻量级数据库

### 架构
- **三层架构**:
  - `SlimTrack.Client` - Blazor WebAssembly 前端
  - `SlimTrack.Server` - ASP.NET Core Web API 后端
  - `SlimTrack.Shared` - 共享数据模型和 DTO

## ?? 项目结构

```
slim-track/
├── SlimTrack.Client/              # Blazor WebAssembly 前端项目
│   ├── Pages/
│   │   └── Index.razor    # 主页面（体重记录表单）
│   ├── Shared/
│   │   ├── EntriesTable.razor    # 数据列表组件
│   │   └── WeightChart.razor     # 图表组件
│   ├── Services/
│   │   └── WeightService.cs      # API 服务封装
│   └── wwwroot/
│    ├── index.html         # 入口 HTML
│       └── css/app.css           # 样式文件
│
├── SlimTrack.Server/       # ASP.NET Core 后端项目
│   ├── Program.cs    # 服务配置和 API 端点
│   ├── AppDbContext.cs      # EF Core 数据库上下文
│   └── appdata/
│    └── slimtrack.db          # SQLite 数据库文件
│
└── SlimTrack.Shared/         # 共享项目
  └── WeightEntryDto.cs         # 数据传输对象
```

## ?? 快速开始

### 前置要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- 任意现代浏览器

### 安装和运行

1. **克隆仓库**
   ```bash
   git clone https://github.com/ydfk/slim-track.git
   cd slim-track
   ```

2. **运行后端服务**
   ```bash
   cd SlimTrack.Server
   dotnet run
   ```
   后端服务将运行在 `https://localhost:5001` 或配置的端口。

3. **运行前端（如果单独部署）**
   ```bash
   cd SlimTrack.Client
   dotnet run
   ```

4. **访问应用**
   
   在浏览器中打开应用地址（通常是 `https://localhost:5001` 或后端配置的地址）。

### 数据库初始化

应用首次运行时会自动创建 SQLite 数据库文件（位于 `SlimTrack.Server/appdata/slimtrack.db`）。

如需重置数据库，只需删除该文件并重启服务。

## ?? API 端点

### 获取体重记录列表
```http
GET /api/weights?start=2024-01-01&end=2024-12-31
```

### 获取统计数据
```http
GET /api/weights/stats?days=30
```
返回最近 N 天的最小值、最大值、平均值和数据点。

### 新增或更新记录
```http
POST /api/weights
Content-Type: application/json

{
  "date": "2024-01-15",
  "weightKg": 70.5,
  "note": "早餐后"
}
```
**注意**：同一日期只能有一条记录（Upsert 操作）。

### 删除记录
```http
DELETE /api/weights/{id}
```

## ?? 使用说明

1. **记录体重**：在首页表单中选择日期、输入体重（1-500kg），可选填备注
2. **查看趋势**：页面中部的图表展示最近 60 天的体重变化趋势
3. **管理数据**：在数据列表中查看所有记录，支持删除操作
4. **更新记录**：选择已存在的日期并提交，将自动更新该日期的数据

## ?? 配置说明

### 数据库路径
修改 `SlimTrack.Server/Program.cs` 中的数据库路径：
```csharp
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "appdata", "slimtrack.db");
```

### 图表显示天数
修改 `SlimTrack.Client/Pages/Index.razor` 中的天数参数：
```razor
<WeightChart Days="60" />
```

## ?? 数据模型

### WeightEntry（数据库实体）
- `Id` (int) - 主键
- `Date` (DateOnly) - 记录日期（唯一索引）
- `WeightKg` (decimal) - 体重（千克，精度 5,2）
- `Note` (string?) - 备注（最大 200 字符）
- `CreatedAt` (DateTime) - 创建时间
- `UpdatedAt` (DateTime) - 更新时间

## ?? 贡献

欢迎提交 Issue 和 Pull Request！

## ?? 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。

## ?? 作者

**ydfk**

- GitHub: [@ydfk](https://github.com/ydfk)
- Repository: [slim-track](https://github.com/ydfk/slim-track)

## ?? 致谢

感谢所有开源社区的贡献者和支持者！

---

**Happy Tracking! ??**
