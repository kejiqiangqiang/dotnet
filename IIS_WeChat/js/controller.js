/**
 * Created by Administrator on 2016/11/7.
 */
angular.module('myApp', ['ionic'], function ($httpProvider) {
    $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
    var param = function (obj) {
        var query = '', name, value, fullSubName, subName, subValue, innerObj, i;
        for (name in obj) {
            value = obj[name];
            if (value instanceof Array) {
                for (i = 0; i < value.length; ++i) {
                    subValue = value[i];
                    fullSubName = name + '[' + i + ']';
                    innerObj = {};
                    innerObj[fullSubName] = subValue;
                    query += param(innerObj) + '&';
                }
            }
            else if (value instanceof Object) {
                for (subName in value) {
                    subValue = value[subName];
                    fullSubName = name + '[' + subName + ']';
                    innerObj = {};
                    innerObj[fullSubName] = subValue;
                    query += param(innerObj) + '&';
                }
            }
            else if (value !== undefined && value !== null)
                query += encodeURIComponent(name) + '=' + encodeURIComponent(value) + '&';
        }
        return query.length ? query.substr(0, query.length - 1) : query;
    };
    // Override $http service's default transformRequest
    $httpProvider.defaults.transformRequest = [function (data) {
        return angular.isObject(data) && String(data) !== '[object File]' ? param(data) : data;
    }];
})

  .controller('projectCtrl', function ($scope, $timeout, HttpFactory, myNote, CacheFactory, RequestUrl, $q) {
      //得到微信提供的随机code，用来请求access_token
      var code = window.location.search.substring(6, window.location.search.indexOf('&'));

      getAccessToken();

      //得到access_token,并读取用户信息
      function getAccessToken() {
          HttpFactory.send({
              url: RequestUrl + 'WeiXinBLL.GetAccessToken',
              method: 'post',
              data: {
                  code:code
              }
          }).success(function (data) {
              CacheFactory.save('openid', data.openid);
              CacheFactory.save('access_token', data.access_token);
              checkInfo(data.openid).then(function (result) {
                  //debugger;
                  //如果没有数据，从微信服务器获取用户数据
                  if (result == "null") {
                      GetUserInfo(data.access_token, data.openid);
                  } else {
                      //绑定数据
                      $scope.user = result.data;
                  }
              });
          });
      }

      //显示加载过程和内容
      $scope.load = { isLoading: false, result: "" };

      //项目申报
      $scope.projectSubmit = function () {

          if (!validate()) {
              return;
          }
          $scope.load = { isLoading: true, result: "正在申报" };
          var dataValue={
              model_project: JSON.stringify($scope.project),
              devices: JSON.stringify($scope.devices),
              model_user: JSON.stringify($scope.user)
          };
          HttpFactory.send({
              url: RequestUrl + 'ProjectBLL.Add',
              data: dataValue,
              method: 'post'
          }).success(function (data) {
              console.log(data);
              $scope.load = { isLoading: true, result: data.message };
              $timeout(function () {
                  $scope.load = { isLoading: false, result: "" };
              }, 2000);
          });
      }

      //验证工作
      function validate() {
           
          if (!($scope.devices[1].amount === $scope.devices[2].amount)) {
              myNote.myNotice("冷冻泵和冷却泵的台数必须相等");
              return false;
          }
          if ($scope.project.ProjectName == "") {
              myNote.myNotice("项目名称不能为空");
              return false;
          }
          if (!$scope.user) {
              myNote.myNotice("正在读取用户信息，请稍后重试");
              return false;
          }
          return true;
      }

      //检查用户是否已经在服务器存在，如果存在则不从服务器读取数据
      function checkInfo(openid) {
          var defer = $q.defer();
          HttpFactory.send({
              url: RequestUrl+'UserInfoBLL.GetUserByOpenId',
              data: {
                  openid: openid
              },
              method: 'post'
          }).success(function (data) {
              defer.resolve(data);
          });
          return defer.promise;
      }

      //从微信服务器获得用户信息
      function GetUserInfo(acctoken, openId) {
          HttpFactory.send({
              url: RequestUrl + 'WeiXinBLL.GetUserInfoFromWeChat',
              data: {
                  acc_token: acctoken,
                  openid: openId
              },
              method: 'post'
          }).success(function (data) {
              console.log(data);
              //将用户信息绑定到页面
              $scope.user = {
                  openid: data.openid,
                  nickname: data.nickname,
                  Area: data.province + data.city,
                  headimgurl: data.headimgurl,
                  Mobile: '',
                  EMail: ''
              };
          });
      }


      $scope.toggleSelectState = function (e) {
          $scope.isSelected = !$scope.isSelected;
          e.stopPropagation();
      };

      $scope.hiddenSelectContainer = function () {
          $scope.isSelected = false;
      };

      $scope.currentSelected = { class: "series", value: "串联" };
      $scope.project = { Model: 1, ProjectName: "" };
      //模式选择
      $scope.select = function (value) {
          if (value === 1) {
              $scope.currentSelected = { class: "series", value: "串联" };
          } else {
              $scope.currentSelected = { class: "parallel", value: "并联" };
          }
          $scope.project.Model = value;
      };

      //设施状况
      $scope.devices = [
        { deviceType: 8, deviceTypeName: "主机", amount: 1 },
        { deviceType: 9, deviceTypeName: "冷冻泵", amount: 1 },
        { deviceType: 10, deviceTypeName: "冷却泵", amount: 1 },
        { deviceType: 11, deviceTypeName: "冷却塔", amount: 1 }
      ];

      //修改数目
      $scope.toggleValue = function (index, value) {
          //得到原值
          var orgObj = $scope.devices[index];
          var orgValue = parseInt(orgObj.amount);
          //得到新值
          var newValue = orgValue == null || orgValue !== orgValue ? 0 + value : orgValue + value;
          //不能小于1
          $scope.devices[index].amount = newValue > 0 && newValue < 6 ? newValue : orgValue;
      };

      /*
       *得到位置信息
      */
      function getPosition() {
          if (navigator.geolocation) {
              navigator.geolocation.getCurrentPosition(showPosition, showError, { enableHighAccuracy: true });
          } else {
              alert("Geolocation is not supported by this browser.");
          }
      }

      //api调用成功的回调方法
      function showPosition(position) {
          console.log("Latitude: " + position.coords.latitude + "Longitude: " + position.coords.longitude);
          baiduLocation(position.coords.latitude, position.coords.longitude);
      }

      //api调用失败的回调方法
      function showError(err) {
          alert("定位出错");
      }

      //注意：在得到准确位置前，需要对原生的坐标系转化成百度的坐标系
      function baiduLocation(lat, lon) {
          var point = new BMap.Point(lon, lat);
          //translateCallback(point);
          BMap.Convertor.translate(point, 0, translateCallback);
      }

      //调用百度的逆地址解析
      function translateCallback(point) {
          console.log(point);
          var geoc = new BMap.Geocoder();
          geoc.getLocation(point, function (rs) {
              createDetailAddr(rs);
          }, function (err) {
              alert("location error");
          });
      }

      //创建位置详细信息
      function createDetailAddr(data) {
          var addrDatas = [];
          var addr;
          var pois;
          addr = data.address;
          pois = data.surroundingPois;
          if (pois == null) {
              addrDatas.push(addr);
          } else {
              var isArray = angular.isArray(pois);
              if (isArray) {
                  if (pois.length == 0) {
                      addrDatas.push(addr);
                  } else {
                      for (var i = 0; i < pois.length; i++) {
                          if (pois[i].title) {
                              addrDatas.push(addr + pois[i].title);
                          }
                      }
                  }
              } else {
                  if (pois.title) {
                      addrDatas.push(addr + pois.title);
                  }
              }
          }
          //默认选中第一条信息
          console.log(addrDatas);
          alert(addrDatas);
      }
  })

  .factory('HttpFactory', function ($http, $ionicPopup, $ionicLoading) {
      var send = function (config) {
          !!config.scope && (config.scope.loading = true);
          !!config.mask && $ionicLoading.show({
              template: typeof config.mask == "boolean" ? '请稍等...' : config.mask
          });
          config.headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
          config.method == 'post' && (config.data = config.data || {});
          ionic.extend(config, {
              timeout: 15000
          });
          var http = $http(config);
          http.catch(function (error) {
              if (error.status == 0) {

              }
              else if (status == 403) {
                  error.data = {
                      template: '/(ㄒoㄒ)/~~403'
                  }
              } else {
                  error.data = {
                      template: '错误信息都在这了：' + JSON.stringify(error.data)
                  }
              }
              $ionicPopup.alert({
                  title: '悲剧了...',
                  template: error.data.template,
                  buttons: [
                      {
                          text: '算了',
                          type: 'button-balanced'
                      }
                  ]
              });
          });
          http.finally(function () {
              !!config.scope && (config.scope.loading = false);
              !!config.mask && $ionicLoading.hide();
          });
          return http;
      };
      return {
          send: send
      }
  })

  .factory('CacheFactory', function ($window) {
        var append = function (key, value) {
        };
        var save = function (key, value) {
            $window.localStorage.setItem(key, typeof value == 'object' ? JSON.stringify(value) : value);
        };
        var get = function (key) {
            return $window.localStorage.getItem(key) || null;
        };
        var remove = function (key) {
            $window.localStorage.removeItem(key);
        };
        var removeAll = function () {
            $window.localStorage.clear();
        };
        return {
            append: append,
            save: save,
            get: get,
            remove: remove,
            removeAll: removeAll
        };
    })

  .factory('myNote', function ($ionicLoading, $timeout) {
        return {
            myNotice: function (msg, timeout, prev, post) {
                $ionicLoading.show({ template: msg });
                $timeout(function () {
                    prev && prev();
                    $ionicLoading.hide();
                    post && post();
                }, timeout || 2000);
                return false;
            }
        }
    })
    
  .constant('RequestUrl', 'Action.ashx?Name=WeChat.Business.')
;
