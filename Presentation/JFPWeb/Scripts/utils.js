/// <reference path="../jquery/jquery-1.11.3.min.js" />

(function () {
    /// <summary>Permite crear un alert con Bootstrap.</summary>
    /// <param name="content" type="String|jQuery Object">Contenido a agregar en el Alert.</param>
    /// <param name="prority" type="String">info/success/warning/danger.</param>
    /// <param name="behavior" type="String">append/prepend</param>
    /// <returns type="jQuery Object">Objeto jQuery del alert creado.</returns>
    $.fn.createAlert = function (content, priority, behavior) {

        content = content || "<b>Hello stranger!!!</b>";
        priority = priority || "info";
        behavior = behavior || "append";

        var $elements = this;
        var alertMarkup =
            "<div class=\"alert alert-dismissible fade in\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\">" +
            "<span aria-hidden=\"true\">&times;</span>" +
            "</button>" +
            "</div>";

        return $elements.map(function () {
            var $alert = $(alertMarkup).addClass("alert-" + priority).append(content);
            $(this)[behavior]($alert);

            return $alert[0];
        });
    };

    /// <summary>Busca en un array el valor suministrado, si no existe lo agrega pero si existe lo quita.</summary>
    /// <param name="array" type="Array">Array a evaluar.</param>
    /// <param name="value" type="Any">Contenido a buscar en el array.</param>
    window.toggleArray = function(array, value) {
        var index = array.indexOf(value);

        if (index === -1) {
            array.push(value);
        } else {
            array.splice(index, 1);
        }
    }

    /// <summary>Permite envolver un string en dos palabras.</summary>
    /// <param name="wordA" type="String">Palabra que irá al inicio del String.</param>
    /// <param name="wordB" type="String">Palabra que irá al final del String.</param>
    /// <returns type="String">String envuelto en las dos palabras.</returns>
    String.prototype.wrap = function (wordA, wordB) {
        return wordA + this + wordB;
    };

    /// <summary>Permite convertir los valores de los controles de un formulario a un objeto.</summary>
    /// <returns type="Object">Objeto con los valores del formulario.</returns>
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    /// <summary>
    /// Permite crear options a uno o varios selects u optgroups por medio de un objeto o arreglo.
    /// .option(options)
    ///     (options: [Array, Object]) Data para llenar el select.
    ///
    /// .option(options, mark)
    ///     (options: [Array, Object]) Data para llenar el select.
    ///     (mark: [Boolean]) Si es falso, los objetos que contentan selected serán establecidos por propiedad. Si es true, se agregará el atributo selected en el option.
    ///
    /// .option(options, config)
    ///     (options: [Array, Object]) Data para llenar el select.
    ///     (config : [Object]) Configuración del plugin.
    ///
    /// .option(options, mapper)
    ///     (options: [Array, Object]) Data para llenar el select.
    ///     (mapper: [Function]) Función que se llama con los datos en options. Esta debe retornar la estructura de options.
    ///
    /// .option(options, mapper, config)
    ///     (options: [Array, Object]) Data para llenar el select.
    ///     (mapper: [Function]) Función que se llama con los datos en options. Esta debe retornar la estructura de options.
    ///     (config : [Object]) Configuración del plugin.
    ///
    /// options
    ///         group    : [String] Permite crear un grupo para el option. Si ya existe, simplemente se pone debajo de este.
    ///         text     : [Number, String] Texto a mostrar en el option. Requerido.
    ///         value    : [Number, String] Valor del option. Requerido.
    ///         selected : [Boolean] Permite marcar el option como seleccionado.
    ///
    /// config
    ///         callback : [Function(option)] Función que se llama con el option a agregar en el select. Dentro de la function, this se refiere al option.
    ///         mark     : [Boolean]) Si es falso, los objetos que contengan selected serán establecidos por propiedad. Si es true, se agregará el atributo selected en el option.
    ///         replace  : [Boolean]) Si es true, remueve todos los elementos dentro del select u optgroup. Si es false hace append.
    ///
    ///
    ///</summary>
    /// <returns type="jQuery Object">Elementos en el selector.</returns>
    $.fn.option = function (options, mapper, config) {

        if ($.isPlainObject(options)) {
            options = [options];
        }

        if (!$.isArray(options)) {
            throw "The options parameter must be an object or array.";
        }

        var defaults = {
            callback: null,
            mark: false,
            replace: false
        }

        if ($.isPlainObject(mapper)) {
            $.extend(defaults, mapper);
        } else if ($.isFunction(mapper)) {
            options = $.map(options, mapper);
            if ($.isPlainObject(config)) {
                $.extend(defaults, config);
            }
        } else {
            defaults.mark = !!mapper;
        }

        var $selects = $(this);

        $selects.each(function () {
            var $select = $(this);

            if (!$select.is("select") && !$select.is("optgroup")) {
                return;
            }

            if (!!defaults.replace) {
                $select.empty();
            }

            $.each(options, function () {
                var option = this;

                if ($.isEmptyObject(option)) {
                    return;
                }

                var $option = $("<option />");

                $option.attr("value", option.value);

                $option.text(option.text);

                if (!!defaults.mark) {
                    $option.attr("selected", !!option.selected);
                } else {
                    $option.prop("selected", !!option.selected);
                }

                if (typeof (defaults.callback) === "function") {
                    defaults.callback.call($option[0], $option[0]);
                }

                if ($select.is("select") && (option.group || "").toString().length > 0) {
                    var $optgroup = $select.find("optgroup[label='" + option.group + "']");

                    if ($optgroup.length === 0) {
                        $optgroup = $("<optgroup />");
                        $optgroup.attr("label", option.group);
                    };

                    $optgroup.append($option).appendTo($select);

                } else {
                    $select.append($option);
                }

            });
        });

        return $selects;
    };

    /// <summary>
    /// Permite ejecutar una función de acuerdo a un tiempo, comunmente usada en eventos como keyup o input. 
    /// .delay(name, callback, ms)
    ///     (name: [String]) Requerido. Un identificador para el delay.
    ///     (callback: [Function]) Requerido. La función a ejecutar.
    ///     (ms: [Number]) El tiempo en milisegundos para ejecutar la función. Por defecto 0.
    ///</summary>
    window.delay = (function () {
        var logTimer = {};
        return function (name, callback, ms) {
            if (!name) {
                throw new Error("The parameter name is required.");
            };
            if (!callback) {
                throw new Error("The parameter callback is required.");
            };
            if (typeof callback !== "function") {
                throw new Error(callback + " is not a function.");
            };
            ms = ms || 0;

            clearTimeout(logTimer[name] || 0);
            logTimer[name] = setTimeout(callback, ms);
        };
    })();

    /// <summary>
    /// Permite saber si una variable esta definida como global retornando una promesa, done si existe fail si no existe.
    /// .varReady(varName)
    ///     (varName: [String]) Requerido. Nombre de la variable a evaluar.
    ///</summary>
    window.varReady = function (varName) {
        var $q = $.Deferred();
        var delay = 100;
        var counter = 50;

        setTimeout(function checkFb() {
            if (!(varName in window)) {

                if (counter <= 0)
                    $q.reject();
                else
                    setTimeout(checkFb, delay);

                counter--;
            } else
                $q.resolve();

        }, delay - delay);

        return $q.promise();
    };

    /// <summary>
    /// Permite saber si un string es un JSON valido.
    /// .isValid(data)
    ///     (data: [String]) Requerido. String a evaluar.
    ///</summary>
    JSON.isValid = function (data) {
        try {
            return !!this.parse(data);
        } catch (e) {
            return false;
        }
    }

    var diacriticsMap = {
        '\u00C0': 'A', // À => A
        '\u00C1': 'A', // Á => A
        '\u00C2': 'A', // Â => A
        '\u00C3': 'A', // Ã => A
        '\u00C4': 'A', // Ä => A
        '\u00C5': 'A', // Å => A
        '\u00C6': 'AE', // Æ => AE
        '\u00C7': 'C', // Ç => C
        '\u00C8': 'E', // È => E
        '\u00C9': 'E', // É => E
        '\u00CA': 'E', // Ê => E
        '\u00CB': 'E', // Ë => E
        '\u00CC': 'I', // Ì => I
        '\u00CD': 'I', // Í => I
        '\u00CE': 'I', // Î => I
        '\u00CF': 'I', // Ï => I
        '\u0132': 'IJ', // Ĳ => IJ
        '\u00D0': 'D', // Ð => D
        '\u00D1': 'N', // Ñ => N
        '\u00D2': 'O', // Ò => O
        '\u00D3': 'O', // Ó => O
        '\u00D4': 'O', // Ô => O
        '\u00D5': 'O', // Õ => O
        '\u00D6': 'O', // Ö => O
        '\u00D8': 'O', // Ø => O
        '\u0152': 'OE', // Œ => OE
        '\u00DE': 'TH', // Þ => TH
        '\u00D9': 'U', // Ù => U
        '\u00DA': 'U', // Ú => U
        '\u00DB': 'U', // Û => U
        '\u00DC': 'U', // Ü => U
        '\u00DD': 'Y', // Ý => Y
        '\u0178': 'Y', // Ÿ => Y
        '\u00E0': 'a', // à => a
        '\u00E1': 'a', // á => a
        '\u00E2': 'a', // â => a
        '\u00E3': 'a', // ã => a
        '\u00E4': 'a', // ä => a
        '\u00E5': 'a', // å => a
        '\u00E6': 'ae', // æ => ae
        '\u00E7': 'c', // ç => c
        '\u00E8': 'e', // è => e
        '\u00E9': 'e', // é => e
        '\u00EA': 'e', // ê => e
        '\u00EB': 'e', // ë => e
        '\u00EC': 'i', // ì => i
        '\u00ED': 'i', // í => i
        '\u00EE': 'i', // î => i
        '\u00EF': 'i', // ï => i
        '\u0133': 'ij', // ĳ => ij
        '\u00F0': 'd', // ð => d
        '\u00F1': 'n', // ñ => n
        '\u00F2': 'o', // ò => o
        '\u00F3': 'o', // ó => o
        '\u00F4': 'o', // ô => o
        '\u00F5': 'o', // õ => o
        '\u00F6': 'o', // ö => o
        '\u00F8': 'o', // ø => o
        '\u0153': 'oe', // œ => oe
        '\u00DF': 'ss', // ß => ss
        '\u00FE': 'th', // þ => th
        '\u00F9': 'u', // ù => u
        '\u00FA': 'u', // ú => u
        '\u00FB': 'u', // û => u
        '\u00FC': 'u', // ü => u
        '\u00FD': 'y', // ý => y
        '\u00FF': 'y', // ÿ => y
        '\uFB00': 'ff', // ﬀ => ff
        '\uFB01': 'fi', // ﬁ => fi
        '\uFB02': 'fl', // ﬂ => fl
        '\uFB03': 'ffi', // ﬃ => ffi
        '\uFB04': 'ffl', // ﬄ => ffl
        '\uFB05': 'ft', // ﬅ => ft
        '\uFB06': 'st' // ﬆ => st
    };

    /// <summary>
    /// Permite remover cualquier tipo de acentos en un string.
    /// .removeDiacritics(str)
    ///     (str: [String]) Requerido. String a eliminar sus acentos.
    ///</summary>
    window.removeDiacritics = function (str) {
        var returnStr = '';

        if (!!str) {
            for (var i = 0; i < str.length; i++) {
                if (diacriticsMap[str[i]]) {
                    returnStr += diacriticsMap[str[i]];
                } else {
                    returnStr += str[i];
                }
            }
        }
        return returnStr;
    }

    /// <summary>
    /// Permite limpiar todos los campos dentro de un formulario.
    ///</summary>
    $.fn.resetForm = function () {
        var $elements = $(this);

        $elements.each(function (key, value) {
            if ($(value).is("form")) {
                value.reset();
            }
        });

        return $elements;
    };

    /// <summary>
    /// Devuelve una nueva fecha con la cantidad de días proporcionados.
    /// .addDays(days)
    ///     (str: [Number]) Requerido. Número de días.
    ///</summary>
    Date.prototype.addDays = function (days) {
        var date = new Date((this).getTime());
        date.setDate(date.getDate() + days);
        return date;
    };

    /// <summary>
    /// Devuelve una nueva fecha con la cantidad de meses proporcionados.
    /// .addDays(months)
    ///     (str: [Number]) Requerido. Número de meses.
    ///</summary>
    Date.prototype.addMonths = function (months) {
        var date = new Date((this).getTime());
        date.setMonth(date.getMonth() + months);
        return date;
    };

    /// <summary>
    /// Permite hacer scroll al top de la página.
    ///</summary>
    window.scrollTop = function () {
        return $("html, body").animate({ scrollTop: 0 }, 600, "swing");
    };

    /// <summary>
    /// Permite hacer scroll al top del elemento.
    ///</summary>
    /// .scrollToElement(positionTop)
    ///     (positionTop: [Number]) Requerido. Posición top en pixeles del elemento.
    ///</summary>
    window.scrollToElement = function (positionTop) {
        $("html, body").animate({ scrollTop: positionTop - 60 }, 600, "swing");
    };

    /// <summary>
    /// Crea un objeto queryString de acuerdo a la url.
    ///</summary>
    (function () {
        var match,
            pl = /\+/g,  // Regex for replacing addition symbol with a space
            search = /([^&=]+)=?([^&]*)/g,
            decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
            query = window.location.search.substring(1);

        window.queryString = {};
        while ((match = search.exec(query)))
            window.queryString[decode(match[1])] = decode(match[2]);
    })();

    /// <summary>
    /// Devuelve un string regex con todos los acentos posibles.
    ///</summary>
    window.includeRegexAccents = function (query) {
        var expresion = query;

        expresion = expresion.replace(/[a\341\301\340\300\342\302\344\304]/ig, "[a\341\301\340\300\342\302\344\304]");
        expresion = expresion.replace(/[e\351\311\350\310\352\312\353\313]/ig, "[e\351\311\350\310\352\312\353\313]");
        expresion = expresion.replace(/[i\355\315\354\314\356\316\357\317]/ig, "[i\355\315\354\314\356\316\357\317]");
        expresion = expresion.replace(/[o\363\323\362\322\364\324\366\326]/ig, "[o\363\323\362\322\364\324\366\326]");
        expresion = expresion.replace(/[u\372\332\371\331\373\333\374\334]/ig, "[u\372\332\371\331\373\333\374\334]");
        expresion = expresion.replace(/[c\347\307]/ig, "[c\347\307]");

        return expresion;
    }

    /// <summary>
    /// Permite obtener la extensión de un archivo (string).
    ///</summary>
    window.getExtension = function(filename) {
        return filename.substr((~-filename.lastIndexOf(".") >>> 0) + 2).toLowerCase();
    }

    /// <summary>
    /// Permite cambiar la extensión de un archivo (string).
    ///</summary>
    window.changeExtension = function(filename, extension) {
        return filename.substr(0, (~-filename.lastIndexOf(".") >>> 0) + 2) + extension;
    }

})();
