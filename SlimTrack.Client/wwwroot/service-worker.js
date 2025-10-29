// Service Worker for PWA
const CACHE_NAME = "slimtrack-v1";
const urlsToCache = ["/", "/index.html", "/css/app.css", "/manifest.json", "/icon-192.png", "/icon-512.png"];

// 安装 Service Worker
self.addEventListener("install", (event) => {
  event.waitUntil(caches.open(CACHE_NAME).then((cache) => cache.addAll(urlsToCache)));
});

// 激活 Service Worker
self.addEventListener("activate", (event) => {
  event.waitUntil(
    caches.keys().then((cacheNames) => {
      return Promise.all(
        cacheNames.map((cacheName) => {
          if (cacheName !== CACHE_NAME) {
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
});

// 拦截请求
self.addEventListener("fetch", (event) => {
  event.respondWith(
    caches.match(event.request).then((response) => {
      // 缓存命中则返回缓存，否则发起网络请求
      return response || fetch(event.request);
    })
  );
});
