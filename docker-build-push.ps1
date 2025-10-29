#!/usr/bin/env pwsh
# PowerShell 脚本 - 构建 Docker 镜像并可选推送到 Docker Hub

param(
    [Parameter(Mandatory=$false)]
    [string]$Version = "latest",

    [Parameter(Mandatory=$false)]
    [switch]$Push = $false
)

$ErrorActionPreference = "Stop"

# 固定的 Docker Hub 用户名
$DockerHubUsername = "ydfk"
$imageName = "$DockerHubUsername/slim-track"

# 颜色输出函数
function Write-ColorOutput($ForegroundColor) {
    $fc = $host.UI.RawUI.ForegroundColor
    $host.UI.RawUI.ForegroundColor = $ForegroundColor
    if ($args) {
        Write-Output $args
    }
    $host.UI.RawUI.ForegroundColor = $fc
}

Write-ColorOutput Green "========================================="
if ($Push) {
    Write-ColorOutput Green "  SlimTrack 构建并推送到 Docker Hub"
} else {
    Write-ColorOutput Green "  SlimTrack Docker 镜像构建"
}
Write-ColorOutput Green "========================================="

# 步骤 1: 清理旧的构建产物
Write-ColorOutput Cyan "`n步骤 1: 清理旧的构建产物..."
if (Test-Path "publish") {
    Remove-Item -Recurse -Force publish
    Write-ColorOutput Green "✓ 已清理 publish 目录"
} else {
    Write-ColorOutput Green "✓ 无需清理"
}

# 步骤 2: 构建 Docker 镜像
Write-ColorOutput Cyan "`n步骤 2: 构建 Docker 镜像..."
Write-ColorOutput White "  镜像名称: ${imageName}:${Version}"
docker build -t ${imageName}:$Version .
if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput Red "✗ Docker 镜像构建失败"
    exit 1
}

# 如果不是 latest，也打上 latest 标签
if ($Version -ne "latest") {
    docker tag ${imageName}:$Version ${imageName}:latest
    Write-ColorOutput Green "✓ 已创建 latest 标签"
}

Write-ColorOutput Green "✓ Docker 镜像构建成功"

# 如果仅构建，显示结果并退出
if (-not $Push) {
    Write-ColorOutput Green "`n========================================="
    Write-ColorOutput Green "  ✓ 构建完成！"
    Write-ColorOutput Green "========================================="

    Write-ColorOutput Cyan "`n镜像信息："
    docker images ${imageName}:$Version

    Write-ColorOutput Cyan "`n后续操作："
    Write-ColorOutput White "  1. 本地运行测试:"
    Write-ColorOutput Yellow "     docker run -d -p 8080:8080 -v ./appdata:/app/appdata --name slim-track ${imageName}:$Version"

    Write-ColorOutput White "`n  2. 推送到 Docker Hub:"
    Write-ColorOutput Yellow "     .\docker-build-push.ps1 -Version $Version -Push"

    Write-ColorOutput Green "`n✓ 所有步骤完成！"
    exit 0
}

# 步骤 3: 登录 Docker Hub
Write-ColorOutput Cyan "`n步骤 3: 登录 Docker Hub..."
Write-ColorOutput Yellow "请输入 Docker Hub 密码或访问令牌："
docker login -u $DockerHubUsername
if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput Red "✗ Docker Hub 登录失败"
    exit 1
}
Write-ColorOutput Green "✓ 登录成功"

# 步骤 4: 推送镜像到 Docker Hub
Write-ColorOutput Cyan "`n步骤 4: 推送镜像到 Docker Hub..."
docker push ${imageName}:$Version
if ($LASTEXITCODE -ne 0) {
    Write-ColorOutput Red "✗ 镜像推送失败"
    exit 1
}

if ($Version -ne "latest") {
    Write-ColorOutput Cyan "正在推送 latest 标签..."
    docker push ${imageName}:latest
    if ($LASTEXITCODE -ne 0) {
        Write-ColorOutput Red "✗ latest 标签推送失败"
        exit 1
    }
}

# 完成
Write-ColorOutput Green "`n========================================="
Write-ColorOutput Green "  ✓ 推送完成！"
Write-ColorOutput Green "========================================="
Write-ColorOutput Cyan "`n镜像信息："
Write-ColorOutput White "  - Docker Hub: https://hub.docker.com/r/$DockerHubUsername/slim-track"
Write-ColorOutput White "  - 镜像名称: $imageName"
Write-ColorOutput White "  - 版本标签: $Version"
if ($Version -ne "latest") {
    Write-ColorOutput White "  - Latest 标签: 是"
}

Write-ColorOutput Cyan "`n使用方法："
Write-ColorOutput White "  docker pull ${imageName}:$Version"
Write-ColorOutput White "  docker run -d -p 8080:8080 -v ./appdata:/app/appdata --name slim-track ${imageName}:$Version"

Write-ColorOutput Green "`n✓ 所有步骤完成！"
