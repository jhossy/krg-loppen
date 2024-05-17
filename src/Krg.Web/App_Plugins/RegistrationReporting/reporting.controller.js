angular.module('umbraco').controller('RegistrationsPluginController', // Scope object is the main object which is used to pass information from the controller to the view.
    function ($scope, $http, editorState, contentResource, overlayService) {
        $scope.aRegistrations = [];
        $scope.exportDocument = {}
        $scope.loading = false;

        var vm = this;
        vm.CurrentNodeId = editorState.current.id;
        vm.CurrentNodeAlias = editorState.current.contentTypeAlias;

        var fetchYear = contentResource.getById(vm.CurrentNodeId).then(function (node) {
            var properties = node.variants[0].tabs[0].properties;

            $scope.exportYear = properties[0].value; //exportYear is first property on this document type
        });

        $scope.getRegistrations = function () {
            $scope.loading = true;
            $scope.aRegistrations = $http({
                method: 'GET',
                url: '/umbraco/backoffice/api/registrations/getregistrations/?year=' + $scope.exportYear
            }).then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                console.log($scope.loading);
                $scope.loading = false;
                console.log($scope.loading);
                $scope.aRegistrations = response.data;
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                $scope.loading = false;
                console.log(response);
            });
        };

        $scope.exportToExcel = function () {
            $scope.loading = true;
            $scope.exportDocument = $http({                
                method: 'GET',
                url: '/umbraco/backoffice/api/registrations/exportasexcel/?year=' + $scope.exportYear,
                responseType: 'blob'
            }).then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                $scope.loading = false;
                var file = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var contentDisposition = response.headers('content-disposition');
                var fileName = 'fallback.xslx';
               
                if (contentDisposition.includes(';') &&
                    contentDisposition.includes('filename') &&
                    contentDisposition.includes('=')) {
                    fileName = contentDisposition.split(';')[1].split('filename')[1].split('=')[1].trim();
                }
                
                var fileURL = URL.createObjectURL(file);
                var a = document.createElement('a');
                a.href = fileURL;
                a.target = '_blank';
                a.download = fileName;
                document.body.appendChild(a); //create the link "a"
                a.click(); //click the link "a"
                document.body.removeChild(a); //remove the link "a"
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                $scope.loading = false;
                console.log(response);
            });
        }

        $scope.delete = function (id) {
            var options = {
                title: 'Slet tilmelding',
                content: 'Er du sikker p&aring; at du vil slette denne tilmelding?',
                disableBackdropClick: false,
                disableEscKey: false,
                submit: function () {
                    $scope.aRegistrations = $http({
                            method: 'POST',
                            url: '/umbraco/backoffice/api/registrations/delete/?id=' + id + '&year=' + $scope.exportYear,
                        }).then(function successCallback(response) {
                            $scope.aRegistrations = response.data;
                        }, function errorCallback(response) {
                            // called asynchronously if an error occurs
                            // or server returns response with an error status.
                            console.log(response);
                        });

                    overlayService.close();
                }
            };

            overlayService.confirmDelete(options);
        }

    });

