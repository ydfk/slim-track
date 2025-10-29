# SlimTrack 发布指南

## 快速发布

### Windows
```powershell
.\publish.ps1
```

### Linux
```powershell
.\publish.ps1 linux-x64
```

### macOS
```powershell
.\publish.ps1 osx-x64
```

## 手动发布

### Windows (64位)
```bash
dotnet publish SlimTrack.Server\SlimTrack.Server.csproj -c Release -r win-x64 -o publish\win-x64
```

### Linux (64位)
```bash
dotnet publish SlimTrack.Server/SlimTrack.Server.csproj -c Release -r linux-x64 -o publish/linux-x64
```

### macOS (64位 Intel)
```bash
dotnet publish SlimTrack.Server/SlimTrack.Server.csproj -c Release -r osx-x64 -o publish/osx-x64
```

### macOS (ARM64 Apple Silicon)
```bash
dotnet publish SlimTrack.Server/SlimTrack.Server.csproj -c Release -r osx-arm64 -o publish/osx-arm64
```

## 发布配置说明

项目已配置为单文件发布,主要特性:

- **PublishSingleFile**: 打包为单个可执行文件
- **SelfContained**: 包含 .NET 运行时,无需安装 .NET
- **RuntimeIdentifier**: win-x64 (可修改为其他平台)
- **PublishTrimmed**: false (保留完整功能,文件较大)
- **IncludeNativeLibrariesForSelfExtract**: true (包含 SQLite 等原生库)

## 运行发布的应用

### Windows
```bash
cd publish\win-x64
.\SlimTrack.Server.exe
```

### Linux/macOS
```bash
cd publish/linux-x64  # 或 osx-x64
chmod +x SlimTrack.Server
./SlimTrack.Server
```

然后在浏览器中访问: http://localhost:5205

## 发布文件说明

发布后的主要文件:
- `SlimTrack.Server.exe` (Windows) 或 `SlimTrack.Server` (Linux/macOS) - 主程序
- `appsettings.json` - 配置文件
- `wwwroot/` - 前端静态文件 (Blazor WASM)
- `appdata/` - 数据库文件 (运行时自动创建)

## 优化建议

### 减小文件大小
如果想要更小的文件,可以启用裁剪:

```xml
<PublishTrimmed>true</PublishTrimmed>
```

注意: 裁剪可能导致某些反射功能失效,建议充分测试。

### 跨平台发布
修改 csproj 文件,移除 `<RuntimeIdentifier>` 属性,然后在发布时指定:

```bash
dotnet publish -c Release -r <runtime> -o publish/<runtime>
```

## 部署注意事项

1. 首次运行会自动创建数据库
2. 数据库文件位于 `appdata/slimtrack.db`
3. 可以修改 `appsettings.json` 配置端口和其他设置
4. 建议在生产环境使用反向代理 (如 Nginx)
