(function () {
    angular.module("precedente", ["ngSanitize", "sf.treeRepeat", "ui.bootstrap", "localytics.directives", "angular.utilities"])
    .filter("replaceAll", function () {
        return function (text, actualString, newString) {
            return text.replace(new RegExp(actualString, 'g'), newString);
        };
    });
})();