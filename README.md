# SlimTrack

一个基于 .NET 9 和 Blazor WebAssembly 构建的体重记录与追踪应用。

## 📖 项目简介

SlimTrack 是一个简单易用的体重记录与管理工具，帮助用户追踪体重变化趋势。应用采用前后端分离架构，提供直观的数据可视化图表和便捷的数据管理功能。

## ✨ 主要功能

- 📝 体重记录（添加、更新和删除）
- 📊 可自定义日期的数据可视化图表
- 📈 体重变化趋势统计
- 🔍 支持日期范围查询
- 📅 每日备注记录
- 📱 **PWA 支持** - 可安装为类似原生 App 的应用
- 🎯 假分页 - 表格每页显示 20 条数据
- ✏️ 点击编辑 - 点击表格行即可编辑该条记录
- 📏 双单位支持 - 斤/公斤自由切换
- 🔄 图表方向切换 - 横向/竖向展示

## 🛠️ 技术栈

### 前端
- **Blazor WebAssembly** - 基于 .NET 9 的客户端 Web 框架
- **C# 13.0** - 最新的 C# 语言特性
- **Chart.js** - 数据可视化（通过 WeightChart 组件）
- **PWA** - 渐进式 Web 应用支持

### 后端
- **ASP.NET Core 9** - Web API 框架
- **Entity Framework Core** - ORM 框架
- **SQLite** - 轻量级数据库

### 项目结构
- **多项目解决方案**:
  - `SlimTrack.Client` - Blazor WebAssembly 前端
  - `SlimTrack.Server` - ASP.NET Core Web API 后端
  - `SlimTrack.Shared` - 共享数据模型和 DTO

## 📁 项目结构

```
slim-track/
├── SlimTrack.Client/              # Blazor WebAssembly 前端项目
│   ├── Pages/
│   │   └── Index.razor            # 主页面（体重记录界面）
│   ├── Shared/
│   │   ├── EntriesTable.razor     # 数据列表组件（支持分页和点击编辑）
│   │   └── WeightChart.razor      # 图表组件（支持横向/竖向切换）
│   ├── Services/
│   │   └── WeightService.cs       # API 服务封装
│   └── wwwroot/
│       ├── index.html             # 入口 HTML（包含 PWA 配置）
│       ├── manifest.json          # PWA 清单文件
│       ├── service-worker.js      # 离线缓存支持
│       ├── icon-*.png             # PWA 应用图标
│       └── css/app.css            # 样式文件
│
├── SlimTrack.Server/              # ASP.NET Core 后端项目
│   ├── Program.cs                 # 服务配置和 API 端点
│   ├── AppDbContext.cs            # EF Core 数据库上下文
│   └── appdata/
│       └── slimtrack.db           # SQLite 数据库文件
│
├── SlimTrack.Shared/              # 共享项目
│   └── WeightEntryDto.cs          # 数据传输对象
│
├── generate-icons.ps1             # PWA 图标生成脚本
├── PWA_GUIDE.md                   # PWA 使用指南
└── docker-compose.yml             # Docker 部署配置
```

## 🚀 快速开始

### 前置要求

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- 任意代码编辑器

### 安装运行

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
   后端服务将在 `https://localhost:5001` 或配置的端口。

3. **访问应用**
   
   在浏览器中打开应用地址（通常是 `https://localhost:5001` 或配置的地址）。

### 数据库初始化

应用首次运行时会自动创建 SQLite 数据库文件，位于 `SlimTrack.Server/appdata/slimtrack.db`。

如需重置数据库，只需删除该文件并重新运行。

## 🐳 Docker 部署

### 使用 Docker Compose

```bash
# 构建并运行
docker-compose up -d

# 访问应用
# http://localhost:8080
```

### 使用 Docker 镜像

```bash
# 拉取镜像
docker pull ydfk/slim-track:latest

# 运行容器
docker run -d -p 8080:8080 -v ./appdata:/app/appdata ydfk/slim-track:latest
```

## 📱 PWA 支持 (渐进式 Web 应用)

SlimTrack 现已支持 PWA 功能，可以像原生 App 一样安装到手机和电脑上！

### ✨ 主要特性

- ✅ **离线访问** - Service Worker 缓存支持
- ✅ **桌面图标** - 添加到主屏幕
- ✅ **全屏体验** - 无浏览器地址栏
- ✅ **自动适配** - iOS/Android/Desktop 全平台
- ✅ **主题优化** - 状态栏颜色自定义

### 📱 在 iPhone 上安装

1. 使用 **Safari 浏览器** 打开网站
2. 点击底部分享按钮 📤
3. 选择 **"添加到主屏幕"**
4. 点击 **"添加"**

### 🤖 在 Android 上安装

1. 使用 **Chrome 浏览器** 打开网站
2. 点击提示横幅或菜单中的 **"安装应用"**
3. 确认安装

### 💻 在桌面端安装

1. 使用 Chrome/Edge 打开网站
2. 点击地址栏的安装图标 ⊕
3. 确认安装

### 📖 详细说明

查看 [PWA_GUIDE.md](PWA_GUIDE.md) 获取完整的 PWA 功能说明和使用指南。

## 🔌 API 端点

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
  "weightGongJin": 70.5,
  "note": "早晨"
}
```
**注意**：同一日期只能有一条记录（Upsert 操作）。

### 删除记录
```http
DELETE /api/weights/{id}
```

## 📝 使用说明

### 基本操作

1. **记录体重**：在主页面中选择日期、输入体重（1-1000斤）、可选备注
2. **查看趋势**：切换到图表标签页，展示最近 60 天的体重变化趋势
3. **管理数据**：在数据列表中查看所有记录，支持删除操作
4. **更新记录**：选择已存在的日期并提交，会自动更新该日期的数据

### 高级功能

#### 点击编辑
- 直接点击表格中的任意一行
- 该条记录的数据会自动填充到顶部表单
- 修改后点击"保存/更新"即可更新

#### 图表切换
- **横向/竖向切换**：点击"📊 竖向"/"📈 横向"按钮
- **单位切换**：点击"📏 斤"/"📏 公斤"按钮

#### 分页浏览
- 表格自动分页，每页显示 20 条记录
- 支持上一页/下一页和直接跳转

## ⚙️ 配置说明

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

### 分页大小
修改 `SlimTrack.Client/Shared/EntriesTable.razor` 中的分页大小：
```csharp
private int pageSize = 20; // 修改为需要的数量
```

## 📊 数据模型

### WeightEntry（数据库实体）
- `Id` (int) - 主键
- `Date` (DateOnly) - 记录日期（唯一索引）
- `WeightJin` (decimal) - 体重（斤）
- `WeightGongJin` (decimal) - 体重（公斤）
- `Note` (string?) - 备注（最长 200 字符）
- `CreatedAt` (DateTime) - 创建时间
- `UpdatedAt` (DateTime) - 更新时间

### WeightEntryDto（传输对象）
```csharp
public record WeightEntryDto(
    int Id,
    DateOnly Date,
    decimal WeightJin,      // 斤
    decimal WeightGongJin,  // 公斤
    string? Note
);
```

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

本项目采用 MIT 许可证，详见 [LICENSE](LICENSE) 文件。

## 👤 作者

**ydfk**

- GitHub: [@ydfk](https://github.com/ydfk)
- Repository: [slim-track](https://github.com/ydfk/slim-track)

## 🙏 致谢

感谢所有开源项目的贡献者和支持者！

---

**Happy Tracking! 💪📊**
