(function () {
    // declaring the module in one file / anonymous function
    // (only pass a second parameter THIS ONE TIME as a redecleration creates bugs
    // which are very hard to dedect)
    angular.module('angular.utilities', [])
        .config(['$httpProvider', function ($httpProvider) {
            $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        }])
        .filter("buildArray", ["$rootScope", function ($rootScope) {
            //Builds an array from array in object array.
            return function (collection, property) {
                var newArray = [];
                collection.forEach(function (value) {
                    newArray = newArray.concat(value[property]);
                });

                if (angular.equals($rootScope.newArrayFromObjectArray, newArray))
                    return $rootScope.newArrayFromObjectArray;
                else
                    $rootScope.newArrayFromObjectArray = newArray;

                return newArray;
            };
        }])
        .filter('characters', function () {
            return function (input, chars, breakOnWord, filter) {
                if (isNaN(chars)) return input;
                if (chars <= 0) return '';
                if (!!filter) return input;
                if (input && input.length > chars) {
                    input = input.substring(0, chars);

                    if (!breakOnWord) {
                        var lastspace = input.lastIndexOf(' ');
                        //get last space
                        if (lastspace !== -1) {
                            input = input.substr(0, lastspace);
                        }
                    } else {
                        while (input.charAt(input.length - 1) === ' ') {
                            input = input.substr(0, input.length - 1);
                        }
                    }
                    return input + '…';
                }
                return input;
            };
        })
        .filter('formatBytes', function () {
            var $config = {
                // Byte units following the IEC format
                // http://en.wikipedia.org/wiki/Kilobyte
                units: [{
                    size: 1073741824,
                    suffix: ' GB'
                }, {
                    size: 1048576,
                    suffix: ' MB'
                }, {
                    size: 1024,
                    suffix: ' KB'
                }]
            };
            return function (bytes) {
                if (!angular.isNumber(bytes)) {
                    return '';
                }
                var unit = true,
                    i = 0,
                    prefix,
                    suffix;
                while (unit) {
                    unit = $config.units[i];
                    prefix = unit.prefix || '';
                    suffix = unit.suffix || '';
                    if (i === $config.units.length - 1 || bytes >= unit.size) {
                        var fixedNumber = (bytes / unit.size).toFixed(3);
                        fixedNumber = fixedNumber.substring(0, fixedNumber.length - 1);
                        return prefix + parseFloat(fixedNumber) + suffix;
                    }
                    i += 1;
                }
                return '';
            };
        })
        .filter('grouped', ['$rootScope', function ($rootScope) {
            //Sirve para agrupar los datos, para más información: https://groups.google.com/forum/#!topic/angular/LjwlVYMulN8
            return function (items, count) {
                if (!count)
                    count = 1;

                if (!angular.isArray(items) && !angular.isString(items)) return items;

                var array = [];
                for (var i = 0; i < items.length; i++) {
                    var chunkIndex = parseInt(i / count, 10);
                    var isFirst = (i % count === 0);
                    if (isFirst)
                        array[chunkIndex] = [];
                    array[chunkIndex].push(items[i]);
                }

                if (angular.equals($rootScope.arrayinSliceOf, array))
                    return $rootScope.arrayinSliceOf;
                else
                    $rootScope.arrayinSliceOf = array;

                return array;
            };
        }])
        .filter('joinBy', function () {
            return function (input, delimiter) {
                return (input || []).join(delimiter || ',');
            };
        })
        .filter("paginate", function () {
            return function (collection, currentPage, itemsPerPage, scope, name) {
                var inicio = ((currentPage * itemsPerPage) - itemsPerPage);
                var fin = (currentPage * itemsPerPage);

                var _collection = collection.slice(inicio, fin);

                var paginatorInfo = {
                    totalItems: collection.length,
                    remaining: collection.length - _collection.length
                };

                if (angular.isObject(scope)) {
                    if (angular.isString(name))
                        scope[name] = paginatorInfo;
                    else
                        scope.paginatorInfo = paginatorInfo;
                }

                return _collection;
            };
        })
        .filter("parseDate", function () {
            //Las fechas en JSON vienen en un formato String muy diferente... acá se arreglan dichos Strings a fechas. Sino viene en este formato, se intenta convertir dicho string a una fecha.
            var re = /\/Date\(([0-9]*)\)\//;
            return function (x) {
                var m = x.match(re);
                if (m)
                    return new Date(parseInt(m[1]));
                else {
                    var someDate = new Date(x);
                    return !!someDate.getTime() ? someDate : null;
                }
            };
        })
        .filter('orderObjectBy', function () {
            return function (items, field, reverse) {
                var filtered = [];
                angular.forEach(items, function (item) {
                    filtered.push(item);
                });
                filtered.sort(function (a, b) {
                    return (a[field] > b[field] ? 1 : -1);
                });
                if (reverse) filtered.reverse();
                return filtered;
            };
        })
        .filter('words', function () {
            return function (input, words, filter) {
                if (isNaN(words)) return input;
                if (words <= 0) return '';
                if (!!filter) return input;
                if (input) {
                    var inputWords = input.split(/\s+/);
                    if (inputWords.length > words) {
                        input = inputWords.slice(0, words).join(' ') + '…';
                    }
                }
                return input;
            };
        })
        .directive('fileChange', ['$parse', function ($parse) {
            return {
                restrict: 'A',
                link: function ($scope, element, attrs) {

                    // Get the function provided in the file-change attribute.
                    // Note the attribute has become an angular expression,
                    // which is what we are parsing. The provided handler is
                    // wrapped up in an outer function (attrHandler) - we'll
                    // call the provided event handler inside the handler()
                    // function below.
                    var attrHandler = $parse(attrs.fileChange);

                    // This is a wrapper handler which will be attached to the
                    // HTML change event.
                    var handler = function (e) {

                        $scope.$apply(function () {

                            // Execute the provided handler in the directive's scope.
                            // The files variable will be available for consumption
                            // by the event handler.
                            attrHandler($scope, {
                                $event: e,
                                $files: e.target.files
                            });
                        });
                    };

                    // Attach the handler to the HTML change event
                    element[0].addEventListener('change', handler, false);
                }
            };
        }])
        .directive("focusOn", function () {
            return {
                restrict: "A",
                link: function (scope, element, attrs) {
                    scope.$watch(attrs.focusOn, function (value) {
                        if (!!value) {
                            setTimeout(function () {
                                element.focus();
                            });
                        }
                    });
                }
            }
        })
        .directive("ngCompare", function () {
            return {
                restrict: 'A',
                require: '?ngModel',
                link: function (scope, elem, attr, ngModel) {
                    if (!ngModel) return;

                    var otherField = null;

                    scope.$watch(attr.ngCompare, function (newVal) {
                        otherField = newVal;
                        ngModel.$validate();
                    });

                    ngModel.$validators.compare = function (modelValue, viewValue) {
                        return modelValue === otherField;
                    };
                }
            };
        })
        .directive('ngEnter', ["$parse", function ($parse) {
            return function (scope, element, attrs) {
                var attrEnter = $parse(attrs.ngEnter);

                element.on("keypress", function (event) {
                    if (event.which === 13) {
                        scope.$apply(function () {
                            attrEnter(scope, {
                                $event: event
                            });
                        });
                    }
                });
            };
        }])
        .directive('ngEsc', ["$parse", function ($parse) {
            return function (scope, element, attrs) {
                var attrEnter = $parse(attrs.ngEsc);

                element.on("keydown", function (event) {
                    if (event.which === 27) {
                        scope.$apply(function () {
                            attrEnter(scope, {
                                $event: event
                            });
                        });
                    }
                });
            };
        }])
        .directive('ngRepeatPost', ["$parse", function ($parse) {
            return function (scope, element, attrs) {
                if (scope.$last) {
                    $parse(attrs.ngRepeatPost)(scope, {});
                }
            };
        }]);
})();
