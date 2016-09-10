var LegisMarker = function (options) {
    options = options || {};
    options.dataOnChange = options.dataOnChange || new Function();

    // Variables.
    var mouseX = 0;
    var mouseY = 0;
    var maxRemarkText = 255;
    // Lista de los 
    var selectionsArray = [];
    var mainSelector;
    var lastMouseUp = null;
    // El rango actualmente seleccionado.
    var currentRange;
    var $closeButton = null;
    var currentDocument;
    var currentSuscriptor;

    // Inicialización.
    this.init = function (documentSelector) {

        mainSelector = documentSelector;

        // Agregar el ícono del resaltado (oculto).
        $(mainSelector).append('<a id="txtselect_marker"></a>');

        // Evento click sobre el ícono de resaltado.
        $('#txtselect_marker').on('click', function() {
            RemoveHitMarks();
            Resaltar(true);
            UpdateMyRemarksList();
        });

        // Evento click sobre el documento.
        $(mainSelector).on("mousedown", function (e) {
            HideMarker();
        });

        // Evento mouseUp para presentar el resaltador.
        $(mainSelector).on("mouseup", function (e) {
            if (lastMouseUp != null && (((+new Date()) - lastMouseUp) < 1500))
                return;

            lastMouseUp = +new Date();

            // Capturar la posición del mouse.
            var ParentOS = $(this).parent().parent().parent().offset();
            mouseX = e.pageX - ParentOS.left;
            mouseY = e.pageY - ParentOS.top;

            var Seleccion = GetSelection();
            if (Seleccion.toString().length == 0)
                return;

            // Determinar si la selección no se sobrepone con otro resaltado.
            if (!IsValidSelection(Seleccion.getRangeAt(0)))
                return;

            // Presentar el marcador.
            setTimeout(function () { ShowMarker(); }, 250);
        });

        $closeButton = $('<a class="txtsel_close" href="#" style="display: none; position: absolute;" data-markindex />').appendTo(document.body);
    };

    function getCoords(elem) { // crossbrowser version
        var box = elem.getBoundingClientRect();

        var body = document.body;
        var docEl = document.documentElement;

        var scrollTop = window.pageYOffset || docEl.scrollTop || body.scrollTop;
        var scrollLeft = window.pageXOffset || docEl.scrollLeft || body.scrollLeft;

        var clientTop = docEl.clientTop || body.clientTop || 0;
        var clientLeft = docEl.clientLeft || body.clientLeft || 0;

        var top = box.top + scrollTop - clientTop;
        var left = box.left + scrollLeft - clientLeft;

        return { top: Math.round(top), left: Math.round(left) };
    }

    // True: La selección es válida (no contiene o hace parte de otros resaltados)
    function IsValidSelection(Rango) {

        var Current = Rango.startContainer;

        while (true) {
            if ($(Current).parents('.Resaltado_Seleccion').length)
                return false;

            if (Current == Rango.endContainer)
                break;

            Current = GetNextNodeInTree(Current);
        }
        return true;
    }

    // Obtener la selección actual.
    function GetSelection() {
        var NodeSelection;
        if (document.getSelection) {
            NodeSelection = document.getSelection();
        }
        else if (window.getSelection) {
            NodeSelection = window.getSelection();
        }
        else if (document.selection && document.selection.type != "Control") {
            NodeSelection = document.selection;
        }
        return NodeSelection;
    }

    // Hacer visible el ícono de resaltado.
    function ShowMarker() {
        currentRange = GetSelection().getRangeAt(0);
        var icon = document.getElementById('txtselect_marker');
        icon.style.top = (mouseY - 30) + 'px';
        icon.style.left = (mouseX - 20) + 'px';
        icon.className = 'show';
    }

    // Ocultar el ícono de resaltado.
    function HideMarker() {
        var icon = document.getElementById('txtselect_marker');
        if (icon)
            icon.className = '';
    }

    // Resaltar.
    function Resaltar(save) {

        var data = GetSelectionData(currentRange);
        var rangeText = currentRange.toString();
        var Rango = currentRange;
        var arrayIndex = selectionsArray.length;

        if (Rango.startContainer == Rango.endContainer) {
            // La selección está TODA dentro de un mismo elemento.
            var TextBefore = Rango.startContainer.nodeValue.substring(0, Rango.startOffset);
            var TextInside = Rango.startContainer.nodeValue.substring(Rango.startOffset, Rango.endOffset);
            var TextAfter = Rango.startContainer.nodeValue.substring(Rango.endOffset);

            $(Rango.startContainer).replaceWith(
                TextBefore + '<span class="Resaltado_Seleccion" data-markindex="' + arrayIndex + '">'
                + TextInside + "</span>" + TextAfter);
        }
        else {
            // La selección se extiende a varios elementos.
            var NodeCursor = Rango.startContainer;
            var ThisIsLast, ThisIsFirst;
            var TextBefore;
            var TextInside;
            var TextAfter;
            var TextAfterNode;

            while (true) {
                ThisIsFirst = NodeCursor == Rango.startContainer;
                ThisIsLast = NodeCursor == Rango.endContainer;
                TextBefore = '';
                TextInside = '';
                TextAfter = '';

                // El texto ANTES de la selección.
                if (ThisIsFirst)
                    TextBefore = NodeCursor.nodeValue.substring(0, Rango.startOffset);

                // El texto dentro de la selección.
                if (!ThisIsFirst && !ThisIsLast)
                    TextInside = NodeCursor.nodeValue;
                else if (ThisIsFirst)
                    TextInside = NodeCursor.nodeValue.substring(Rango.startOffset);
                else if (ThisIsLast)
                    TextInside = NodeCursor.nodeValue.substring(0, Rango.endOffset);

                // El texto después de la selección
                if (ThisIsLast)
                    TextAfter = NodeCursor.nodeValue.substring(Rango.endOffset);

                // ================================================================================

                // Poner el texto ANTES de la selección.
                NodeCursor.nodeValue = TextBefore;

                // Poner el texto EN la selección.
                var NewSpan = document.createElement("span");
                $(NewSpan).attr('class', 'Resaltado_Seleccion');
                $(NewSpan).attr('data-markindex', arrayIndex);
                $(NewSpan).text(TextInside);
                $(NewSpan).insertAfter(NodeCursor);

                // Poner el texto DESPUÉS de la selección.
                if (!!TextAfter) {
                    TextAfterNode = document.createTextNode(TextAfter);
                    $(NewSpan).after(TextAfterNode);
                }

                // Salir si ya se llegó al último.
                if (ThisIsLast)
                    break;

                NodeCursor = GetNextNodeInTree(NewSpan);
            }
        }

        // Quitar las selecciones.
        HideMarker();

        // Agregar la seleccion al array de selecciones.
        var arrayIndex = selectionsArray.length;
        var newMark = {
            order: 0,
            index: arrayIndex,
            startNode: data.startNode,
            startOffset: data.startOffset, startSelector: data.startSelector,
            endNode: data.endNode,
            endOffset: data.endOffset, endSelector: data.endSelector,
            rangeText: rangeText
        };
        selectionsArray.push(newMark);

        // Agregar el botón para quitar el resaltado.
        AddRemoveButton(arrayIndex);

        // Enviar al website.
        if (save) {
            SaveMarks();
        }
    }

    // Envía todas las selecciones para grabación.
    function SaveMarks() {
        if (selectionsArray.length == 0)
            return;

        var saveData = [];
        SortMarks();

        for (var i = 0; i < selectionsArray.length; i++) {
            saveData.push(
            {
                StartOffset: selectionsArray[i].startOffset,
                StartSelector: selectionsArray[i].startSelector,
                EndOffset: selectionsArray[i].endOffset,
                EndSelector: selectionsArray[i].endSelector
            });
        }

        jQuery.post("/SentenciaResaltados/InsertResaltado", {
            codSuscriptor: currentSuscriptor,
            codSentencia: currentDocument,
            "": saveData
        });
    }

    // Ordena los rango en órden de aparición en el DOM.
    function SortMarks() {

        // Establecer el orden con base a las apariciones de los resaltados
        // dentro del documento.
        var AllMarks = $("span.Resaltado_Seleccion");
        for (var i = 0; i < AllMarks.length; i++) {
            var markIndex = $(AllMarks[i]).attr("data-markindex");
            for (var j = 0; j < selectionsArray.length ; j++) {
                if (selectionsArray[j].index == markIndex) {
                    selectionsArray[j].order = i;
                }
            }
        }

        for (var i = 0; i < (selectionsArray.length - 1) ; i++)
            for (var j = i + 1; j < selectionsArray.length; j++) {
                var n1 = selectionsArray[i];
                var n2 = selectionsArray[j];
                if (n1.order > n2.order) {
                    var x = selectionsArray[i];
                    selectionsArray[i] = selectionsArray[j];
                    selectionsArray[j] = x;
                }
            }
    }

    // Compara la posición relativa de dos nodos.
    // 0 = iguales, 1 = node1 < node2, 2 = node1 > node2 
    function CompareNodePositions(node1, node2) {
        if (node1.startNode == node2.startNode) {
            if (node1.startOffset == node2.startOffset)
                return 0;
            else if (node1.startOffset < node2.startOffset)
                return 1;
            else
                return 2;
        }
        else if (node1.startNode.compareDocumentPosition(node2.startNode) & 2)
            return 1;
        else
            return 2;
    }

    // Agrega el evento presentar el botón para quitar la selección.
    function AddRemoveButton(index) {
        $("span.Resaltado_Seleccion[data-markindex = '" + index + "']").on("mouseenter", function () {
            var $this = $(this);
            // Si ya está visible el botón no presentarlo.

            // Pintarlo solo sobre el último.
            var Element = $("span.Resaltado_Seleccion[data-markindex = '" + index + "']").last()[0];
            var UltimoResaltado = Element.getClientRects();
            UltimoResaltado = UltimoResaltado[UltimoResaltado.length - 1];

            // Determinar la distancia hasta el mainSelector
            $closeButton.attr("data-markindex", index);
            var PosX = $(Element).offset().left + $(Element).width();
            var PosY = $(Element).offset().top;
            $closeButton.css({ left: PosX, top: PosY }).css("display", "block");

            // Agregar el evento para quitar la selección.
            $closeButton.on("click", function () {
                RemoveMark($(this).attr("data-markindex"));
            });
        });
    }

    function ProgramRemoveButtonRemove(number) {
        setTimeout(function () {
            $('a.txtsel_close[data-markindex = "' + number + '"]').remove();
        }, 2000);
    }

    // Quitar un resaltado.
    function RemoveMark(index) {
        // Los resaltados.
        var spans = $("span.Resaltado_Seleccion[data-markindex = '" + index + "']");
        spans.each(function (key, value) {
            $(value).replaceWith(value.innerHTML);
        });

        // El botón de borrar resaltado
        $('a.txtsel_close[data-markindex = "' + index + '"]').remove();

        // Quitar la selección del array en memoria.
        $(selectionsArray).each(function (key, value) {
            if (value.index == index) {
                selectionsArray.splice(key, 1);
                jQuery.post("/SentenciaResaltados/DeleteResaltado", {
                    CodSentencia: currentDocument,
                    CodSuscriptor: currentSuscriptor,
                    StartOffset: value.startOffset,
                    StartSelector: value.startSelector,
                    EndOffset: value.endOffset,
                    EndSelector: value.endSelector
                });
                UpdateMyRemarksList();
                return;
            }
        });
    }

    // Retorna el siguiente nodo en el árbol HTML.
    function GetNextNodeInTree(cursor) {
        // Recorrer los demás hermanos.
        var r = NavigateNextBrothers(cursor);
        if (r != null)
            return r;

        // Buscar el abuelo.
        cursor = cursor.parentNode.parentNode;
        return GetNextNodeInTree(cursor);
    }

    // Recorrer los hermanos y los sobrinos.
    function NavigateNextBrothers(cursor) {
        var current = NavigateNextSibling(cursor); // cursor.nextSibling;

        while (!!current) {
            if (current.nodeType == 3)
                return current;
            var r = NavigateChildren(current);
            if (r != null)
                return r;

            current = NavigateNextSibling(current); // current.nextSibling;
        }
        return null;
    }

    // Pasa el siguiente sibling teniendo en cuenta todo tipo de nodo.
    function NavigateNextSibling(cursor) {
        var Index = -1;
        var Children;

        do {
            Children = cursor.parentNode.childNodes;

            for (var i = 0; i < Children.length; i++) {
                if (Children[i] == cursor) {
                    Index = i;
                    break;
                }
            }
            Index++;
            cursor = cursor.parentNode;
        } while (Index >= Children.length);
        return Children[Index];
    }

    // Recorrer los hijos.
    function NavigateChildren(cursor) {
        if (cursor.childNodes.length > 0) {
            for (var i = 0; i < cursor.childNodes.length; i++) {
                var nodo = cursor.childNodes[i];
                // Se encontró el nodo tipo texto.
                if (nodo.nodeType == 3)
                    return nodo;
                else if (nodo.childNodes.length > 0) {
                    // Un hijo con hijos, verificar los hijos.
                    var r = GetNextNodeInTree(nodo);
                    if (r != null && r.nodeType == 3)
                        return r;
                }
            }
        }
        return null;
    }

    // Para un área seleccionada genera los datos del selector.
    function GetSelectionData(Rango) {

        // Seleccionar el inicio.
        var NodoStart = Rango.startContainer.parentNode;
        var NodoEnd = Rango.endContainer.parentNode;

        var NodoStartIndex = 0;
        for (var i = 0; i < Rango.startContainer.parentNode.childNodes.length; i++) {
            if (Rango.startContainer.parentNode.childNodes[i] == Rango.startContainer) {
                NodoStartIndex = i;
                break;
            }
        }

        var NodoEndIndex = 0;
        for (var i = 0; i < Rango.endContainer.parentNode.childNodes.length; i++) {
            if (Rango.endContainer.parentNode.childNodes[i] == Rango.endContainer) {
                NodoEndIndex = i;
                break;
            }
        }

        return {
            startNode: Rango.startContainer,
            startOffset: Rango.startOffset,
            startSelector: $(NodoStart).getPath() + '|' + NodoStartIndex,
            endNode: Rango.endContainer,
            endOffset: Rango.endOffset,
            endSelector: $(NodoEnd).getPath() + '|' + NodoEndIndex
        };
    }

    // Recupera todos los marcadores del documento y los presenta.
    this.GetAllMarkers = function (CodSuscriptor, CodSentencia) {
        currentDocument = CodSentencia;
        currentSuscriptor = CodSuscriptor;

        var x = $.getJSON("/SentenciaResaltados/GetAllResaltados?codSuscriptor="
            + CodSuscriptor + "&codSentencia=" + CodSentencia,
            function (datos) {
                DisplayMarkers(datos);
            });
    }

    // Pintar los marcadores recién cargados.
    function DisplayMarkers(data) {
        if (data.length === 0) {
            options.dataOnChange([]);
            return;
        }

        RemoveHitMarks();

        for (var i = 0; i < data.length; i++) {
            try {
                var range = document.createRange();

                var StartData = data[i].StartSelector.split('|');
                var EndData = data[i].EndSelector.split('|');

                range.setStart($(StartData[0])[0].childNodes[StartData[1]], data[i].StartOffset);
                range.setEnd($(EndData[0])[0].childNodes[EndData[1]], data[i].EndOffset);
                currentRange = range;
                Resaltar(false);
            } catch (e) {
            }
        }

        UpdateMyRemarksList(data);

    }

    // Quita todas las marcas de búsqueda en este documento.
    // Esto es importante para que no interfiera con los resaltados.
    function RemoveHitMarks() {
        // Los resaltados.
        var spans = $(".marcoSentencia span.blast[aria-hidden=true]");
        spans.each(function (key, value) {
            $(value).replaceWith(value.innerHTML);
        });
    }

    // Actualizar la lista de "Mis Resaltados".
    function UpdateMyRemarksList() {
        var UL = $("#MisResaltados");
        UL.empty();

        SortMarks();

        for (var i = 0; i < selectionsArray.length ; i++) {
            var text = selectionsArray[i].rangeText.substring(0, maxRemarkText);

            if (selectionsArray[i].rangeText.length > maxRemarkText)
                text = text + "...";

            var $li = $("<li />");
            var $p = $("<p />").text(text).attr("data-mark", selectionsArray[i].index);
            var $a = $("<a/>").addClass("elimina").addClass("elimina-resaltado").attr("title", "Eliminar resaltado").attr("data-mark", selectionsArray[i].index);
            var $i = $("<i />").addClass("fa fa-trash");

            // Borrar un resaltado.
            $a.on("click", function () {
                var a = $(this).attr("data-mark");
                RemoveMark(a);
                $(this).parents("li:first").remove();
            });

            // Desplazarse al resaltado.
            $p.on("click", function () {
                var a = $(this).attr("data-mark");
                var PosY = $("span[data-markindex=" + a + "]").offset().top;
                scrollToElement(PosY);
            });

            $li.append($p);
            $p.append($a);
            $a.append($i);

            UL.append($li);
        }

        options.dataOnChange(selectionsArray);
    }

    // Obtener el selector de un elemento.
    jQuery.fn.extend({
        getPath: function () {
            var path, node = this;
            while (node.length) {
                var realNode = node[0], name = realNode.localName;
                if (!name) break;
                name = name.toLowerCase();

                var parent = node.parent();

                var sameTagSiblings = parent.children(name);
                if (sameTagSiblings.length > 1) {
                    allSiblings = parent.children();
                    var index = allSiblings.index(realNode) + 1;
                    if (index > 1) {
                        name += ':nth-child(' + index + ')';
                    }
                }
                path = name + (path ? '>' + path : '');
                node = parent;

                if (node[0] == $(mainSelector)[0])
                    break;
            }
            return mainSelector + '>' + path;
        }
    });

}