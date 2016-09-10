// jTip Plugin for jQuery - Version 0.1
// by Angel Grablev for Enavu Web Development network (enavu.com)
// Dual license under MIT and GPL :) enjoy
/*

To use simply call .jTip() on the element you wish like so:
$(".tip").jTip(); 

you can specify the following options:
attr = the attribute you want to pull the content from
tip_class = the class you want to style for the tip, make sure to have a width when styling
y_coordinate = the distance from the mouse the tip will show in the vertical direction
x_coordinate = the distance from the mouse the tip will show in the horizontal direction

LIBRERÍA MODIFICADA: Por favor comparela con la última versión de jTip.
*/
(function ($) {
    $.fn.jTip = function (options) {
        var defaults = {
            attr: "title",
            tip_class: "tip_window",
            y_coordinate: 20,
            x_coordinate: 20
        };
        options = $.extend(defaults, options);
        return this.filter(function () {
            //  Si ya un elemento tiene tooltip, ignorelo.
            return !$(this).data("hasTooltip");
        }).each(function () {
            var obj = $(this);
            obj.data("hasTooltip", true);
            var $tip = $('<div class="' + options.tip_class + '" style="position:absolute; z-index:1080; left:-9999px;"></div>');
            $("body").append($tip);
            var tObj = $tip;
            var titleValue = obj.attr(options.attr);
            obj.hover(function (e) {
                titleValue = obj.attr(options.attr);
                tObj.css({
                    opacity: 0.8,
                    display: "none"
                }).fadeIn(400);
                obj.removeAttr(options.attr);
                tObj.css({
                    'left': e.pageX + options.y_coordinate,
                    'top': e.pageY + options.y_coordinate
                }).html(titleValue);
                tObj.stop().fadeTo("10", 0.8);
            }, function (e) {
                obj.attr(options.attr, titleValue);
                tObj.stop().fadeOut(400);
            });
            obj.mousemove(function (e) {
                tObj.css({
                    'top': e.pageY + options.y_coordinate,
                    'left': e.pageX + options.y_coordinate
                });
            });
            //  Se requiere Angular, emite un evento cuando un elemento es eliminado del DOM.
            obj.on("$destroy", function() {
                $tip.remove();
            });
        });
    };
})(jQuery);