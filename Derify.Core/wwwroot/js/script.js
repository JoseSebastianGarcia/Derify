$(document).ready(function(){
    

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
        var links = $(".link");
        
        var lineStyle = "2px solid #AA969696";
        var noneStyle = "none";

        for(var i = 0; i < links.length; i++)
        {
            var link = links[i];
            var sourceId = link.getAttribute("Source");
            var targetId = link.getAttribute("Target");

            if(sourceId != undefined && targetId != undefined && sourceId != null && targetId != null)
            {
                var sourceEl = document.getElementById(sourceId);
                var targetEl = document.getElementById(targetId);

                var sourceRect = sourceEl.getBoundingClientRect();
                var targetRect = targetEl.getBoundingClientRect();
                
                const x1 = sourceRect.left + sourceRect.width / 2;
                const y1 = sourceRect.top + sourceRect.height / 2;
                
                const x2 = targetRect.left + targetRect.width / 2;
                const y2 = targetRect.top + targetRect.height / 2;
                
                var handled = false;
                
                // Determina la posición relativa de Entity1 en relación a Entity2
                if (!handled && x1 < x2 && y1 < y2) {
                    // Entity1 está en el cuadrante superior izquierdo de Entity2
                    // Casuística 1
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-bottom': lineStyle,   // Línea inferior
                        'border-right': noneStyle,     // Línea derecha
                        'border-top':noneStyle,
                        'border-left':lineStyle
                    });
                    handled = true;
                } else if (!handled && x1 === x2 && y1 < y2) {
                    // Entity1 está en la misma línea vertical arriba de Entity2
                    // Casuística 2
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-top': lineStyle   // Línea superior
                    });
                    handled = true;
                } else if (!handled && x1 > x2 && y1 < y2) {
                    // Entity1 está en el cuadrante superior derecho de Entity2
                    // Casuística 3
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-bottom': lineStyle,   // Línea inferior
                        'border-right': lineStyle,     // Línea derecha
                        'border-top':noneStyle,
                        'border-left':noneStyle
                    });
                    handled = true;
                } else if (!handled && x1 < x2 && y1 === y2) {
                    // Entity1 está en la misma línea horizontal a la izquierda de Entity2
                    // Casuística 4
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-left': lineStyle   // Línea izquierda
                    });
                    handled = true;
                } else if (!handled && x1 > x2 && y1 === y2) {
                    // Entity1 está en la misma línea horizontal a la derecha de Entity2
                    // Casuística 6
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-right': lineStyle  // Línea derecha
                    });
                    handled = true;
                } else if (!handled && x1 < x2 && y1 > y2) {
                    // Entity1 está en el cuadrante inferior izquierdo de Entity2
                    // Casuística 7
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-bottom': lineStyle,   // Línea inferior
                        'border-right': lineStyle,     // Línea derecha
                        'border-top':noneStyle,
                        'border-left':noneStyle,
                    });
                    handled = true;
                } else if (!handled && x1 === x2 && y1 > y2) {
                    // Entity1 está en la misma línea vertical debajo de Entity2
                    // Casuística 8
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-bottom': lineStyle   // Línea inferior
                    });
                    handled = true;
                } else if (!handled && x1 > x2 && y1 > y2) {
                    // Entity1 está en el cuadrante inferior derecho de Entity2
                    // Casuística 9
                    $(link).css({
                        left: Math.min(x1, x2),
                        top: Math.min(y1, y2),
                        width: Math.abs(x2 - x1),
                        height: Math.abs(y2 - y1),
                        'border-bottom': lineStyle,   // Línea inferior
                        'border-right': noneStyle,     // Línea derecha
                        'border-top':noneStyle,
                        'border-left':lineStyle,
                    });
                    handled = true;
                }
            }
        }
    }
    
});