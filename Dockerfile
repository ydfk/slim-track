# 多阶段构建 - 构建阶段
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# 复制解决方案和项目文件
COPY global.json ./
COPY SlimTrack.slnx ./
COPY SlimTrack.Server/SlimTrack.Server.csproj SlimTrack.Server/
COPY SlimTrack.Client/SlimTrack.Client.csproj SlimTrack.Client/
COPY SlimTrack.Shared/SlimTrack.Shared.csproj SlimTrack.Shared/

# 恢复依赖项
RUN dotnet restore SlimTrack.Server/SlimTrack.Server.csproj

# 复制所有源代码
COPY . .

# 发布应用程序（AOT 和裁剪优化）
WORKDIR /src/SlimTrack.Server
RUN dotnet publish SlimTrack.Server.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:PublishTrimmed=false \
    /p:EnableCompressionInSingleFile=true

# 运行时阶段 - 使用最小的 Alpine 镜像
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# 安装运行时依赖（健康检查需要 curl）
RUN apk add --no-cache curl

# 创建非 root 用户以提高安全性
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# 从构建阶段复制发布的文件
COPY --from=build /app/publish .

# 创建数据目录
RUN mkdir -p /app/appdata && chown -R appuser:appgroup /app/appdata

# 切换到非 root 用户
USER appuser

# 暴露端口
EXPOSE 8080

# 环境变量
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# 启动应用程序
ENTRYPOINT ["dotnet", "SlimTrack.Server.dll"]
