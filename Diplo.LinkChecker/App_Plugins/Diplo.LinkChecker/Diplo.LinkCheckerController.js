angular.module("umbraco")
    .controller("Diplo.LinkCheckerController",
    function ($scope, $http, assetsService, dialogService, notificationsService) {

        var baseApiUrl = "/Umbraco/Api/LinkChecker/";
        var checkLinksUrl = baseApiUrl + "CheckPage/";
        var getIdsToCheckUrl = baseApiUrl + "GetIdsToCheck/";

        var startId = 1074;
        $scope.checkedPages = [];
        $scope.progress = 0;
        $scope.buttonText = "Start Check";
        $scope.startNodeName = "";
        $scope.startNodeIcon = "";
        $scope.onlyErrors = false;
        $scope.finished = false;
        $scope.showStartMessage = false;

        $scope.pageCnt = $scope.linksCheckedCnt = $scope.linksOkCnt = $scope.linksErrorCnt = 0;

        $scope.startCheck = function () {
            $scope.showStartMessage = true;
            dialogService.contentPicker({
                multiPicker: false,
                callback: function (data) {
                    checkIds(data);
                }
            });
        }

    
        var checkIds = function (data) {

            console.log(data);

            $scope.progress = 0;
            $scope.checkedPages = [];
            $scope.buttonText = "Checking...";
            $scope.startNodeName = data.name;
            $scope.startNodeIcon = data.icon;
            $scope.finished = false;
            $scope.showStartMessage = false;
            $scope.pageCnt = $scope.linksCheckedCnt = $scope.linksOkCnt = $scope.linksErrorCnt = 0;

            $http.get(getIdsToCheckUrl + data.id).
                success(function (data, status, headers, config) {

                    var dataLength = data.length;
                    var cnt = 1;
                  
                    for (var i = 0; i < data.length; i++) {

                        $http.get(checkLinksUrl + data[i]).
                          success(function (data, status, headers, config) {

                              console.log(data);

                              $scope.linksCheckedCnt += data.LinksCount;
                              $scope.linksErrorCnt += data.ErrorCount;
                              $scope.linksOkCnt += data.SuccessCount;
                              $scope.pageCnt++;

                              $scope.checkedPages.push(data);

                              $scope.progress = calculateProgress(i, dataLength);


                              $scope.finished = (cnt++ === dataLength);

                              if ($scope.finished) {
                                  notificationsService.success("Finished!", "Link Check has checked " + $scope.linksCheckedCnt + " links and found " + $scope.linksErrorCnt + " errors.");
                                  $scope.buttonText = "Start Check";
                              }
                          }).
                          error(function (data, status, headers, config) {
                              $scope.pageCnt++;
                          });
                    }
                }).
                error(function (data, status, headers, config) {
                });

        }
      

        var calculateProgress = function (currentPage, totalPages) {
            return parseInt((currentPage / totalPages) * 100)
        };


        $scope.openDetail = function (link) {
            var dialog = dialogService.open({
                template: '/App_Plugins/Diplo.LinkChecker/detail.html',
                dialogData: link, show: true, width: 800
            });
        }
    
    });
