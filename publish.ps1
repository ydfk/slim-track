# SlimTrack 发布脚本
# 用法: .\publish.ps1 [平台]
# 平台选项: win-x64 (默认), linux-x64, osx-x64

param(
    [string]$Runtime = "win-x64"
)

$publishDir = Join-Path $PSScriptRoot "publish\$Runtime"

Write-Host "正在发布 SlimTrack 到 $Runtime 平台..." -ForegroundColor Green
Write-Host "输出目录: $publishDir" -ForegroundColor Cyan

# 清理旧的发布文件
if (Test-Path $publishDir) {
    Write-Host "清理旧的发布文件..." -ForegroundColor Yellow
    Remove-Item $publishDir -Recurse -Force
}

# 发布项目
dotnet publish "$PSScriptRoot\SlimTrack.Server\SlimTrack.Server.csproj" `
    --configuration Release `
    --runtime $Runtime `
    --output $publishDir `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n发布成功!" -ForegroundColor Green
    Write-Host "可执行文件位置: $publishDir" -ForegroundColor Cyan
    
    # 显示主要文件信息
    $exeName = if ($Runtime.StartsWith("win")) { "SlimTrack.Server.exe" } else { "SlimTrack.Server" }
    $exePath = Join-Path $publishDir $exeName
    
    if (Test-Path $exePath) {
        $fileInfo = Get-Item $exePath
        Write-Host "`n主程序: $exeName" -ForegroundColor White
        Write-Host "文件大小: $([math]::Round($fileInfo.Length / 1MB, 2)) MB" -ForegroundColor White
        
        Write-Host "`n运行方式:" -ForegroundColor Yellow
        if ($Runtime.StartsWith("win")) {
            Write-Host "  .\$exeName" -ForegroundColor White
        } else {
            Write-Host "  ./$exeName" -ForegroundColor White
        }
        Write-Host "`n然后在浏览器中打开: http://localhost:5205" -ForegroundColor Cyan
    }
} else {
    Write-Host "`n发布失败!" -ForegroundColor Red
    exit 1
}
