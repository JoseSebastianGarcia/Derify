$(document).ready(function () {
    $("html").on("contextmenu", function (e) {
        // Detiene la acción predeterminada del navegador (mostrar el menú contextual)
        e.preventDefault();
    });
    $("tr").click(function (e) {
        if (e.ctrlKey && e.button == 0) {
            if ($(this).attr("highlight") == undefined || $(this).attr("highlight") == "") {
                $(this).css("background-color", "#FFFFCC");
                $(this).attr("highlight", "1");
            }
            else {
                $(this).css("background-color", "");
                $(this).attr("highlight", "");
            }
        }
    });
    $(".searchButton").click(function () {
        $(".searchPanel").removeClass("hide");
        $("#searchText").focus();
    });

    $(".dimmer").click(function () {
        $(".searchPanel").addClass("hide");
        $("#searchText").val("");
    });

    $("#clearSelection").click(function () {
        $(".searchPanel").addClass("hide");

        //Limpio la búsqueda
        var elementosConTexto = $("tr");
        elementosConTexto.css("background-color", "");
        var elementosConTexto = $(".card");
        elementosConTexto.css("filter", "");
        $("#searchText").val("");
    });

    $(document).keypress(function (evt) {
        if (evt.ctrlKey && evt.code == "KeyB") {
            evt.preventDefault();
            $(".searchPanel").removeClass("hide");
            $("#searchText").focus();
        }

        if (evt.code == "Enter") {
            evt.preventDefault();
            $(".searchPanel").addClass("hide");
            var searchText = $("#searchText").val();

            //Limpio la búsqueda
            var elementosConTexto = $("tr");
            elementosConTexto.css("background-color", "");
            var elementosConTexto = $(".card");
            elementosConTexto.css("filter", "");

            //Realizo nueva búsqueda
            if (searchText.trim() != "") {
                var elementosConTexto = $("tr:contains('" + searchText + "')");
                elementosConTexto.css("background-color", "#FFFFCC");
                var elementosConTexto = $(".card:not(:contains('" + searchText + "'))");
                elementosConTexto.css("filter", "grayscale(80%)");
            }
        }
    });    

    $(".card").draggable({
        scroll: true,
        drag:function()
        {
            LoadLinks();
        }
    });
    
    LoadLinks();

    function LoadLinks()
    {
        $("#svg-container").empty();
        var links = $(".link");
        
        for(var i = 0; i < links.length; i++)
        {
            var link = links[i];
            var sourceId = link.getAttribute("Source");
            var targetId = link.getAttribute("Target");

            if (sourceId != undefined && targetId != undefined && sourceId != null && targetId != null) {
                var sourceEl = document.getElementById(sourceId);
                var targetEl = document.getElementById(targetId);

                var sourceRect = window.getComputedStyle(sourceEl);
                var targetRect = window.getComputedStyle(targetEl);

                var rects = {
                    source: {
                        left: parseFloat(sourceRect.getPropertyValue('left')),
                        top: parseFloat(sourceRect.getPropertyValue('top')),
                        width: parseFloat(sourceRect.getPropertyValue('width')),
                        height: parseFloat(sourceRect.getPropertyValue('height'))
                    },
                    target: {
                        left: parseFloat(targetRect.getPropertyValue('left')),
                        top: parseFloat(targetRect.getPropertyValue('top')),
                        width: parseFloat(targetRect.getPropertyValue('width')),
                        height: parseFloat(targetRect.getPropertyValue('height'))
                    }
                };

                const x1 = rects.source.left + rects.source.width / 2;
                const y1 = rects.source.top + rects.source.height / 2;

                const x2 = rects.target.left + rects.target.width / 2;
                const y2 = rects.target.top + rects.target.height / 2;

                // Crea un elemento SVG para la línea de conexión
                var svgns = "http://www.w3.org/2000/svg";
                var svg = document.createElementNS(svgns, "svg");
                var w = (x1 > x2) ? x1 : x2;
                var h = (y1 > y2) ? y1 : y2;

                svg.setAttribute("width", w + "px");
                svg.setAttribute("height", h + "px");
                svg.setAttribute("style", "margin: 0px; padding: 0px; z-index:-1; position:absolute; left:0px; top:0px;");
                svg.setAttribute("xmlns", svgns);
                svg.setAttribute("version", "1.1");

                var line = document.createElementNS(svgns, "line");
                line.setAttribute("x1", x1);
                line.setAttribute("y1", y1);
                line.setAttribute("x2", x2);
                line.setAttribute("y2", y2);
                line.setAttribute("stroke", "#AA969696");
                line.setAttribute("stroke-width", "2");

                svg.appendChild(line);

                // Agrega el SVG al contenedor
                $("#svg-container").append(svg);
            }
        }
    }
    
});