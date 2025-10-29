# 生成 PWA 图标的 PowerShell 脚本
# 使用 .NET System.Drawing 创建简单的图标

Add-Type -AssemblyName System.Drawing

$outputPath = "SlimTrack.Client\wwwroot"
$backgroundColor = [System.Drawing.Color]::FromArgb(59, 130, 246) # #3b82f6
$textColor = [System.Drawing.Color]::White

function Create-Icon {
    param(
        [int]$size,
        [string]$filename
    )

    $bitmap = New-Object System.Drawing.Bitmap($size, $size)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)

    # 填充背景色
    $brush = New-Object System.Drawing.SolidBrush($backgroundColor)
    $graphics.FillRectangle($brush, 0, 0, $size, $size)

    # 添加文字 "体重"
    $font = New-Object System.Drawing.Font("Microsoft YaHei", [int]($size / 4), [System.Drawing.FontStyle]::Bold)
    $textBrush = New-Object System.Drawing.SolidBrush($textColor)
    $format = New-Object System.Drawing.StringFormat
    $format.Alignment = [System.Drawing.StringAlignment]::Center
    $format.LineAlignment = [System.Drawing.StringAlignment]::Center

    $rect = New-Object System.Drawing.RectangleF(0, 0, $size, $size)
    $graphics.DrawString("体重", $font, $textBrush, $rect, $format)

    # 保存
    $fullPath = Join-Path $outputPath $filename
    $bitmap.Save($fullPath, [System.Drawing.Imaging.ImageFormat]::Png)

    Write-Host "✓ 创建 $filename ($size x $size)" -ForegroundColor Green

    # 清理
    $graphics.Dispose()
    $bitmap.Dispose()
    $brush.Dispose()
    $textBrush.Dispose()
    $font.Dispose()
}

Write-Host "开始生成 PWA 图标..." -ForegroundColor Cyan

try {
    Create-Icon -size 192 -filename "icon-192.png"
    Create-Icon -size 512 -filename "icon-512.png"
    Create-Icon -size 152 -filename "icon-152.png"
    Create-Icon -size 167 -filename "icon-167.png"
    Create-Icon -size 180 -filename "icon-180.png"

    Write-Host "`n全部完成! 图标已保存到 $outputPath" -ForegroundColor Green
    Write-Host "提示: 这些是临时图标，建议使用专业设计工具创建更美观的图标" -ForegroundColor Yellow
}
catch {
    Write-Host "错误: $_" -ForegroundColor Red
    Write-Host "如果遇到问题，请手动创建图标或使用在线工具" -ForegroundColor Yellow
}
