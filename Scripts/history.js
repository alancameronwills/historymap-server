        // Get query parameters
        location.queryParameters = {};
        location.search.substr(1).split("&").forEach(function (pair) {
            if (pair === "") return;
            var parts = pair.split("=");
            location.queryParameters[parts[0]] = parts[1] &&
                decodeURIComponent(parts[1].replace(/\+/g, " "));
        });

// For searching and sorting
function comparable(title) {
    return title.toLocaleLowerCase().replace(/[- '",]+/g, "").replace(/^the/, ""
        .replace(/[âêîôûŵŷ]/,function (c){return {"â":"a","ê":"e","î":"i","ô":"o","û":"u","ŵ":"w","ŷ":"y"}[c];}));
}


function isInNode(element, nodeId) {
    if (!element) return false;
    if (!element.id) return isInNode(element.parentElement, nodeId);
    return element.id == nodeId;
}


function noPropagate(e) {
    if (e.stopPropagation) {
        e.stopPropagation();
    }
    else {
        e.cancelBubble = true;
    }
}

function trimQuotes(s) {
    if (typeof s !== "string" || s.length == 0) return "";
    return s.replace(/^"|"$/gm, '');
}


if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

function hashCode(s) {
    var hash = 0, i, chr;
    if (s.length === 0) return hash;
    for (i = 0; i < s.length; i++) {
        chr = s.charCodeAt(i);
        hash = ((hash << 5) - hash) + chr;
        hash |= 0; // Convert to 32bit integer
    }
    return hash;
};

function p(x) {
    if (x) return x._;
    else return null;
}


var isAdvancedBrowser = function () {
    var div = document.createElement('div');
    return (('draggable' in div) || ('ondragstart' in div && 'ondrop' in div)) && 'FormData' in window && 'FileReader' in window;
}();



function zoomed(lat, long) {
    var f1lat = 52.068478, f1long = -4.747869, f2lat = 52.065769, f2long = -4.753887, diameter = 0.008579508;
    return Math.sqrt(Math.pow(lat - f1lat, 2) + Math.pow(long - f1long, 2))
        + Math.sqrt(Math.pow(lat - f2lat, 2) + Math.pow(long - f2long, 2))
        < diameter;
}
