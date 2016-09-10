$(document).ready(function () {

    var selectorUrl = "data-autocomplete-url";
    var selectorAttrUrl = "[data-autocomplete-url]";

    var selectorSource = "data-autocomplete-source";
    var selectorAttrSource = "[data-autocomplete-source]";

    var selectorSubmit = "[data-autocomplete-submit]";
    var selectorEnter = "[data-autocomplete-enter]";

    var Config = function (data) {
        this.source = data,
        this.minLength = 2,
        this.delay = 100,
        this.autoSelect = false,
        this.matcher = function (item) {
            if (item.toLowerCase().indexOf(this.query.trim().toLowerCase()) != -1) {
                return true;
            }
            if (item.toUpperCase().indexOf(this.query.trim().toUpperCase()) != -1) {
                return true;
            }
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&");
            query = query.replace(/a/ig, "[a\341\301\340\300\342\302\344\304]");
            query = query.replace(/e/ig, "[e\351\311\350\310\352\312\353\313]");
            query = query.replace(/i/ig, "[i\355\315\354\314\356\316\357\317]");
            query = query.replace(/o/ig, "[o\363\323\362\322\364\324\366\326]");
            query = query.replace(/u/ig, "[u\372\332\371\331\373\333\374\334]");
            query = query.replace(/c/ig, "[c\347\307]");

            if (item.toLowerCase().match(query.toLowerCase())) {
                return true;
            }
        }
    };

    function buildAutocomplete(selector, data) {
        var $element = $(selector).typeahead(new Config(data));

        if ($element.is(selectorSubmit)) {
            $element.on("keyup", function (e) {
                if (e.which === 13) {
                    $(this).parents("form").submit();
                }
            });
        }

        if ($element.is(selectorEnter)) {
            var event = jQuery.Event("keypress");
            event.which = 13; // # Some key code value

            $element.on("keyup", function (e) {
                if (e.which === 13) {
                    $(this).trigger(event);
                }
            });
        }
    }

    $(selectorAttrUrl)
        .each(function () {
            var $this = $(this);
            if ($this.is(selectorAttrSource)) { return; }

            var url = $this.attr(selectorUrl);
            if (!url) { return; }

            $.getJSON(url)
                .done(function (data) {
                    buildAutocomplete($this, data);
                });
        });

    $(selectorAttrSource)
        .each(function () {
            var $this = $(this);

            var source = $this.attr(selectorSource);
            if (!source) { return; }

            var $source = $("#" + source);
            var data = JSON.parse($source.text());

            buildAutocomplete($this, data);
        });
});