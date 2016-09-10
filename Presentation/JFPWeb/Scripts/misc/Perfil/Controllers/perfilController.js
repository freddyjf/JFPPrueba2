(function () {
    angular.module("precedente")
       .controller("perfilController", [
           "$scope", "perfilFactory", "$timeout", function ($scope, perfilFactory, $timeout) {
               $scope.formCambiarClave = {};
               $scope.mensaje = "";

               $scope.cambiarClaveModel = {
                   claveActual: "",
                   claveNueva: "",
                   confirmarClaveNueva: ""
               };

               $scope.submit = function () {
                   Spin.start();
                   var model = $scope.cambiarClaveModel;

                   perfilFactory.CambiarClave(model.claveActual, model.claveNueva, model.confirmarClaveNueva).then(function (response) {
                       $scope.cambiarClaveModel = {};
                       $scope.formCambiarClave.$setPristine();
                       $scope.formCambiarClave.$setUntouched();
                       onExecuted(response);
                   }).catch(onExecuted);
               };

               function onExecuted(response) {
                   $scope.mensaje = response.data.Message;
                   Spin.stop();
                   $timeout(function () { $scope.mensaje = ""; }, 3000);
               }
           }
       ]);
})();