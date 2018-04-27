"use strict";function setOfCachedUrls(e){return e.keys().then(function(e){return e.map(function(e){return e.url})}).then(function(e){return new Set(e)})}var precacheConfig=[["/boot.js","3dc946900ae2e270bc1b99eef73d0709"],["/boot.js.gz","fd84e824cad13b271477d99587bb2ae5"],["/index.html","424ace19c25b423ff6069c0d15d0503e"],["/static/css/main.75a6dba1.css","75a6dba1147fe23494259d8169993758"],["/static/css/main.75a6dba1.css.gz","a8827fa6b5d1af5954a22a34d1b9d5b7"],["/static/js/main.0ffc604c.js","202f8f552d8ba591491b2848a15f1a9e"],["/static/js/main.0ffc604c.js.gz","4ee85c834f053ebd249c56b4f8db0a9d"],["/static/media/001.1bab5f59.png","1bab5f593c92362342f3ac305610aa99"],["/static/media/002.2790e145.png","2790e145620a75a25169dee3d9cd77cb"],["/static/media/003.1b3dfb74.png","1b3dfb74fbce8ddf2ac1be08abe39e63"],["/static/media/004.f766c196.png","f766c196108f0a636dcde6b706f13221"],["/static/media/005.c60e7ea0.png","c60e7ea013baf330a66b6fc3c967fd91"],["/static/media/airport.4b86bbb9.png","4b86bbb903534cd020cb9ab00c947d59"],["/static/media/bg_chinamap.9e147f8c.png","9e147f8ce8bbaf9a6f61a04f8d474e0a"],["/static/media/businessBuilding.1638538d.png","1638538ddde50a6c4b410db4f0592a0c"],["/static/media/chemicals.c4a0c8e6.png","c4a0c8e643ce8531e99622bea339c85f"],["/static/media/datacenter.e02b6f81.png","e02b6f81c2694f24a2d95aa479fbac33"],["/static/media/hotel.af600c12.png","af600c1290cc2997c46606f480190ba9"],["/static/media/house.914543bf.png","914543bf072438ab06dc9a95385a50c8"],["/static/media/loading.ead84d74.gif","ead84d746b6ee07ee78dc4243d7349c8"],["/static/media/logo.5d5d9eef.svg","5d5d9eefa31e5e13a6610d9fa7a283bb"],["/vender.js","f1e9fb6f42171d46884f010b6d2cc6bb"],["/vender.js.gz","79eedd54322d9995aa6a94b0b2476968"]],cacheName="sw-precache-v3-sw-precache-webpack-plugin-"+(self.registration?self.registration.scope:""),ignoreUrlParametersMatching=[/^utm_/],addDirectoryIndex=function(e,a){var t=new URL(e);return"/"===t.pathname.slice(-1)&&(t.pathname+=a),t.toString()},cleanResponse=function(e){return e.redirected?("body"in e?Promise.resolve(e.body):e.blob()).then(function(a){return new Response(a,{headers:e.headers,status:e.status,statusText:e.statusText})}):Promise.resolve(e)},createCacheKey=function(e,a,t,n){var c=new URL(e);return n&&c.pathname.match(n)||(c.search+=(c.search?"&":"")+encodeURIComponent(a)+"="+encodeURIComponent(t)),c.toString()},isPathWhitelisted=function(e,a){if(0===e.length)return!0;var t=new URL(a).pathname;return e.some(function(e){return t.match(e)})},stripIgnoredUrlParameters=function(e,a){var t=new URL(e);return t.hash="",t.search=t.search.slice(1).split("&").map(function(e){return e.split("=")}).filter(function(e){return a.every(function(a){return!a.test(e[0])})}).map(function(e){return e.join("=")}).join("&"),t.toString()},hashParamName="_sw-precache",urlsToCacheKeys=new Map(precacheConfig.map(function(e){var a=e[0],t=e[1],n=new URL(a,self.location),c=createCacheKey(n,hashParamName,t,/\.\w{8}\./);return[n.toString(),c]}));self.addEventListener("install",function(e){e.waitUntil(caches.open(cacheName).then(function(e){return setOfCachedUrls(e).then(function(a){return Promise.all(Array.from(urlsToCacheKeys.values()).map(function(t){if(!a.has(t)){var n=new Request(t,{credentials:"same-origin"});return fetch(n).then(function(a){if(!a.ok)throw new Error("Request for "+t+" returned a response with status "+a.status);return cleanResponse(a).then(function(a){return e.put(t,a)})})}}))})}).then(function(){return self.skipWaiting()}))}),self.addEventListener("activate",function(e){var a=new Set(urlsToCacheKeys.values());e.waitUntil(caches.open(cacheName).then(function(e){return e.keys().then(function(t){return Promise.all(t.map(function(t){if(!a.has(t.url))return e.delete(t)}))})}).then(function(){return self.clients.claim()}))}),self.addEventListener("fetch",function(e){if("GET"===e.request.method){var a,t=stripIgnoredUrlParameters(e.request.url,ignoreUrlParametersMatching);(a=urlsToCacheKeys.has(t))||(t=addDirectoryIndex(t,"index.html"),a=urlsToCacheKeys.has(t));!a&&"navigate"===e.request.mode&&isPathWhitelisted(["^(?!\\/__).*"],e.request.url)&&(t=new URL("/index.html",self.location).toString(),a=urlsToCacheKeys.has(t)),a&&e.respondWith(caches.open(cacheName).then(function(e){return e.match(urlsToCacheKeys.get(t)).then(function(e){if(e)return e;throw Error("The cached response that was expected is missing.")})}).catch(function(a){return console.warn('Couldn\'t serve response for "%s" from cache: %O',e.request.url,a),fetch(e.request)}))}});