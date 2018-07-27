
//Format tien cho input (setKeyup)
function setKeyup(id) {
    $(id).keyup(function (ev) {
        if ($(this).val().length) {
            var value = gcRev($(this).val(), ',');
            $(this).val(gcFormatStr(value.toString()));
        }
    });
}
//END Format tien cho input (setKeyup)----------------------------
//unformat tien cho string
function MoneyToInt(string) {
    var num = string.replace(/,/g, "");
    return num;
}
//END unformat tien cho string
function formatTien(el) {
    if ($(el).val().length) {
        var value = gcRev($(el).val(), ',');
        $(el).val(gcFormatStr(value.toString()));
    }
};
/* -------- Chuyển định dạng  ----------*/
function gcRev(str, t1) {
    if (str == null)
        return str;
    if (trim(str).length < 1)
        return str;
    var idx = str.indexOf(t1);
    while (idx > -1) {
        str = str.replace(t1, "");
        idx = str.indexOf(t1);
    }
    return str;
}
//-----------------------------------------------
function gcFormatStr(str) {
    return gcNum(str, ",", ".");
}
/* t1=[,] t2=[.] sử dụng theo số tiếng anh gcNum(str,",",".")*/
function gcNum(str, t1, t2) {
    if (str == null)
        return str;
    if (trim(str).length < 1)
        return str;

    var tmpValue = str;
    var arrString = new Array(Math.round(tmpValue.length / 3) + 2);

    tmpValue = tmpValue.replace(t1, "");

    /* -------- Xử lý các dấu chấm   ----------*/
    var idx = tmpValue.indexOf(t1);
    while (idx > -1) {
        tmpValue = tmpValue.replace(t1, "");
        idx = tmpValue.indexOf(t1);
    }
    /*-----------------------------------------*/

    var idxMinus = tmpValue.indexOf("-");
    if (idxMinus == 0)
        tmpValue = tmpValue.substring(idxMinus + 1, tmpValue.length);
    else if (idxMinus > 0)
        tmpValue = tmpValue.substring(0, idxMinus);
    /*/-----------------remove all dấu [-]-------------------*/
    /* -------- Xử lý các dấu chấm   ----------*/
    var idxM = tmpValue.indexOf("-");
    while (idxM > -1) {
        tmpValue = tmpValue.replace("-", "");
        idxM = tmpValue.indexOf("-");
    }
    /*----------------------------------------*/
    var k = 0;
    var i = 0;
    idx = tmpValue.indexOf(t2);
    var strTail = "";
    if (idx >= 0) {
        var strHead = tmpValue.substring(0, idx);

        var lH = strHead.length;
        if (lH > 3) {
            for (k = lH - 3; k > 0; k = k - 3) {
                arrString[i] = strHead.substring(k, k + 3);
                i++;
            }
            /* ------------------------------------ */
            if (k <= 0) {
                arrString[i] = strHead.substring(0, k + 3);
                i++;
            }
        } /* end lH */
        else {
            arrString[i] = strHead.substring(0, strHead.length);
            i++;
        }

        var strTail = tmpValue.substring(idx + 1, tmpValue.length);
        /* remove all comma (,) */
        var idx2 = strTail.indexOf(t2);
        while (idx2 > -1) {
            strTail = strTail.replace(t2, "");
            idx2 = strTail.indexOf(t2);
        }
    }
    else {
        var strHead = tmpValue;
        var lH = strHead.length;
        if (lH > 3) {
            for (k = lH - 3; k > 0; k = k - 3) {
                arrString[i] = strHead.substring(k, k + 3);
                i++;
            }
            /* ------------------------------------ */
            if (k <= 0) {
                arrString[i] = strHead.substring(0, k + 3);
                i++;
            }
        } /* end lH */
        else {
            arrString[i] = strHead.substring(0, strHead.length);
            i++;
        }
    }

    var j = 0; tmpValue = "";
    if (idxMinus >= 0)
        tmpValue += "-";

    /* concat Head */
    for (j = i - 1; j > 0; j--) {
        tmpValue += arrString[j];
        if (j > 0)
            tmpValue += t1;
    }
    if (i > 0)
        tmpValue += arrString[0];

    /* concar Tail */
    if (idx >= 0) {
        tmpValue += t2;
        tmpValue += strTail;
    }
    return tmpValue;
}
function trim(str) {
    return ltrim(rtrim(str));
}
function ltrim(str) {
    for (var k = 0; k < str.length && isWhitespace(str.charAt(k)) ; k++);
    return str.substring(k, str.length);
}
function rtrim(str) {
    for (var j = str.length - 1; j >= 0 && isWhitespace(str.charAt(j)) ; j--);
    return str.substring(0, j + 1);
}
function isWhitespace(charToCheck) {
    var whitespaceChars = " \t\n\r\f";
    return (whitespaceChars.indexOf(charToCheck) != -1);
}

//string.format

String.prototype.format = function () {
    var args = [].slice.call(arguments);
    return this.replace(/(\{\d+\})/g, function (a) {
        return args[+(a.substr(1, a.length - 2)) || 0];
    });
};
// usage
'{0} world'.format('hello');

// Num to Vietnamese currency format

function formatNumber(num) {
    var parts = num.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}