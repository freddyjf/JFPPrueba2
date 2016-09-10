(function () {
    angular.module("precedente")
    .factory("favoritosFactory", ["$http", function ($http) {
        return {
            ObtenerFavoritos: function () {
                return $http.get("/Favoritos/ObtenerCarpetasFavoritos");
            },
            ObtenerCarpetas: function () {
                return $http.get("/Favoritos/ObtenerCarpetas");
            },
            CrearCarpeta: function (nombreCarpeta) {
                return $http.post("/Favoritos/CrearCarpeta", { nombreCarpeta: nombreCarpeta });
            },
            CrearFavorito: function (codAnalisis, codCarpeta, nombreFavorito) {
                return $http.post("/Favoritos/CrearFavorito", { codAnalisis: codAnalisis, codCarpeta: codCarpeta, nombreFavorito: nombreFavorito });
            },
            RenombrarCarpeta: function (nombreCarpeta, codCarpeta) {
                return $http.post("/Favoritos/RenombrarCarpeta", { nombreCarpeta: nombreCarpeta, codCarpeta: codCarpeta });
            },
            RenombrarFavorito: function (nombreFavorito, codFavorito) {
                return $http.post("/Favoritos/RenombrarFavorito", { nombreFavorito: nombreFavorito, codFavorito: codFavorito });
            },
            MoverFavoritos: function (codFavoritos, codCarpeta) {
                if (!angular.isArray(codFavoritos))
                    codFavoritos = [codFavoritos];

                return $http.post("/Favoritos/MoverFavoritos", { codFavoritos: codFavoritos, codCarpeta: codCarpeta });
            },
            EliminarCarpetas: function (codCarpetas) {
                if (!angular.isArray(codCarpetas))
                    codCarpetas = [codCarpetas];

                return $http.post("/Favoritos/EliminarCarpetas", { codCarpetas: codCarpetas });
            },
            EliminarFavoritos: function (codFavoritos) {
                if (!angular.isArray(codFavoritos))
                    codFavoritos = [codFavoritos];

                return $http.post("/Favoritos/EliminarFavoritos", { codFavoritos: codFavoritos });
            }
        };
    }]);
})();