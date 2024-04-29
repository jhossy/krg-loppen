angular.module('umbraco').controller('RegistrationsPluginController', // Scope object is the main object which is used to pass information from the controller to the view.
    function ($scope, $http, editorState, contentResource) {
        $scope.aRegistrations = [];

        var vm = this;
        vm.CurrentNodeId = editorState.current.id;
        vm.CurrentNodeAlias = editorState.current.contentTypeAlias;

        var fetchYear = contentResource.getById(vm.CurrentNodeId).then(function (node) {
            var properties = node.variants[0].tabs[0].properties;

            $scope.exportYear = properties[0].value; //exportYear is first property on this document type
        });

        $scope.getRegistrations = function () {
            console.log($scope.exportYear);
            $scope.aRegistrations = $http({
                method: 'GET',
                url: '/umbraco/backoffice/api/registrations/getregistrations/?year=' + $scope.exportYear
            }).then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                console.log(angular.fromJson(response.data));
                $scope.aRegistrations = response.data;
            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                console.log(response);
            });
        };

        $scope.exportToExcel = function () {
            console.log('export to excel');
        }        

});