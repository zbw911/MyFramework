// JavaScript Document
function aaAlert(str) {
    var msgw, msgh, bordercolor;
    msgw = 470; //提示窗口的宽度
    msgh = 413; //提示窗口的高度
    bordercolor = "#D3D3D3"; //提示窗口的边框颜色
    bgcolor = "#239DDC";
    titlecolor = "#239DDC"; //提示窗口的标题颜色

    var sWidth, sHeight;
    sWidth = document.body.offsetWidth;
    sHeight = $(window).height();


    var bgObj = document.createElement("div");
    bgObj.setAttribute('id', 'bgDiv');
    bgObj.style.position = "absolute";
    bgObj.style.top = "0";
    bgObj.style.background = "#777";
    bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
    bgObj.style.opacity = "0.6";
    bgObj.style.left = "0";
    bgObj.style.width = document.body.offsetWidth + "px";
    bgObj.style.height = document.body.offsetHeight + "px";
    bgObj.style.zIndex = 100000;
    document.body.appendChild(bgObj);
    var msgObj = document.createElement("div")
    msgObj.setAttribute("id", "msgDiv");
    msgObj.setAttribute("align", "center");
    msgObj.style.position = "absolute";
    msgObj.style.background = "white";
    msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
    msgObj.style.border = "1px solid " + bordercolor;
    msgObj.style.width = msgw + "px";
    msgObj.style.height = msgh + "px";
    msgObj.style.top = (document.documentElement.scrollTop + (sHeight - msgh) / 2) + "px";
    msgObj.style.left = (sWidth - msgw) / 2 + "px";
    msgObj.style.zIndex = 100001;
    var title = document.createElement("h4");
    title.setAttribute("id", "msgTitle");
    title.style.cursor = "pointer";
    title.style.position = "absolute";
    title.style.width = "14px";
    title.style.height = "14px";
    title.className = "close"
    title.innerHTML = '<span></span>';
    title.onclick = function abc() {
        document.body.removeChild(bgObj);
        document.getElementById("msgDiv").removeChild(title);
        document.body.removeChild(msgObj);
    }
    document.body.appendChild(msgObj);
    document.getElementById("msgDiv").appendChild(title);
    var txt = document.createElement("div");
    txt.style.margin = "1em 0"
    txt.setAttribute("id", "msgTxt");
    txt.innerHTML = str;
    document.getElementById("msgDiv").appendChild(txt);

}

