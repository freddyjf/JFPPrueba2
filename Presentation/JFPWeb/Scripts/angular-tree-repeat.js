// angular-tree-repeat - v0.0.1
// modified by @sant123
// Santiago Aguilar Hernández
// Now it has $treeIndex, $treeIsRoot and one way data-binding: node in :: children of treeData
// Include this first to define the module that the directives etc. hang off.
//
(function () {
    'use strict';
    angular.module('sf.treeRepeat', []);
}());

// sf-tree-repeat directive
// ========================
// Like `ng-repeat` but recursive
//
(function () {
    'use strict';
    // (part of the sf.treeRepeat module).
    var mod = angular.module('sf.treeRepeat');

    // Utility to turn the expression supplied to the directive:
    //
    //     a in b of c
    //
    // into `{ value: "a", collection: "b", root: "c" }`
    //

    function parseRepeatExpression(expression) {
        var match = expression.match(/^\s*([\$\w]+)\s+in\s+([\S\s]*)\s+of\s+([\S\s]*)$/);
        if (!match) {
            throw new Error("Expected sfTreepeat in form of" +
              " '_item_ in _collection_ of _root_' but got '" +
              expression + "'.");
        }

        var match2 = match[2].match(/^(::)\s*([\$\w]+)/);

        if (!match2) {
            return {
                value: match[1],
                collection: match[2],
                root: match[3],
                oneBinding: false
            };
        } else {
            return {
                value: match[1],
                collection: match2[2],
                root: match[3],
                oneBinding: true
            };
        }
    }

    // The `sf-treepeat` directive is the main and outer directive. Use this to
    // define your tree structure in the form `varName in collection of root`
    // where:
    //  - varName is the scope variable used for each node in the tree.
    //  - collection is the collection of children within each node.
    //  - root is the root node.
    //
    mod.directive('sfTreepeat', ['$log',
      function ($log) {
          return {
              restrict: 'A',
              // Use a scope to attach the node model
              scope: true,
              // and a controller to communicate the template params to `sf-treecurse`
              controller: ['$scope', '$attrs',
                function TreepeatController($scope, $attrs) {
                    var ident = this.ident = parseRepeatExpression($attrs.sfTreepeat);

                    $scope.$treeIndex = -1;
                    $scope.$treeIsRoot = false;

                    //$log.info("Parsed '%s' as %s", $attrs.sfTreepeat, JSON.stringify(this.ident));
                    // Keep the root node up to date.

                    var theWatch = $scope.$watch(this.ident.root, function (v) {
                        $scope[ident.value] = v;

                        // If oneBinding is true, only evaluate once!
                        if (ident.oneBinding) {
                            theWatch();
                        }
                    });
                }
              ],
              // Get the original element content HTML to use as the recursive template
              compile: function sfTreecurseCompile(element) {
                  var template = element.html();
                  return {
                      // set it in the pre-link so we can use it lower down
                      pre: function sfTreepeatPreLink(scope, iterStartElement, attrs, controller) {
                          controller.template = template;
                      }
                  };
              }
          };
      }
    ]);

    // The `sf-treecurse` directive is a little like `ng-transclude` in that it
    // signals where to insert our recursive template
    mod.directive('sfTreecurse', ['$compile',
      function ($compile) {
          return {
              // which must come from a parent `sf-treepeat`.
              require: "^sfTreepeat",
              link: function sfTreecursePostLink(scope, iterStartElement, attrs, controller) {
                  // Now we stitch together an element containing a vanila repeater using
                  // the values from the controller.

                  // Add :: in the other repeats if oneBinding is true.
                  var withOneBind = (!!controller.ident.oneBinding) ? ":: " : "";

                  var build = [
                    '<', iterStartElement.context.tagName, ' data-ng-repeat="',
                    controller.ident.value, ' in ',
                    withOneBind + controller.ident.value, '.', controller.ident.collection,
                    '">',
                    controller.template,
                    '</', iterStartElement.context.tagName, '>'
                  ];

                  scope.$treeIndex++;
                  scope.$treeIsRoot = scope.$treeIndex === 0;

                  var el = angular.element(build.join(''));

                  // We swap out the element for our new one and tell angular to do its
                  // thing with that.
                  iterStartElement.replaceWith(el);
                  $compile(el)(scope);
              }
          };
      }
    ]);

}());