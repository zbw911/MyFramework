// JavaScript Document
// JavaScript Document
function sAlert(str) {
    var msgw, msgh, bordercolor;
    msgw = 350; //提示窗口的宽度
    msgh = 225; //提示窗口的高度
    bordercolor = "#C1C1C1"; //提示窗口的边框颜色
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
    bgObj.style.zIndex = 100001;
    document.body.appendChild(bgObj);
    var msgObj = document.createElement("div")
    msgObj.setAttribute("id", "msgDiv");
    msgObj.setAttribute("align", "center");
    msgObj.style.position = "absolute";
    msgObj.style.background = "white";
    msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
    msgObj.style.border = "2px solid " + bordercolor;
    msgObj.style.width = msgw + "px";
    msgObj.style.paddingBottom ="0px"
 

    msgObj.height < msgh ? msgh : "auto";
    msgObj.style.zoom = 1;
    msgObj.style.zIndex = 100002;
    msgObj.style.top = (document.documentElement.scrollTop + (sHeight - msgh) / 2) + "px";
    msgObj.style.left = (sWidth - msgw) / 2 + "px";
    var title = document.createElement("h4");
    title.setAttribute("id", "msgTitle");
    title.style.cursor = "pointer";
    title.style.position = "absolute";
    title.style.width = "14px";
    title.style.height = "14px";
    title.className = "alertdiv"
    title.innerHTML = '<span></span>';
    title.onclick = function () {
        document.body.removeChild(bgObj);
        document.getElementById("msgDiv").removeChild(title);
        document.body.removeChild(msgObj);
    }
    document.body.appendChild(msgObj);
    document.getElementById("msgDiv").appendChild(title);
    var txt = document.createElement("div");
    txt.setAttribute("id", "msgTxt");
    txt.innerHTML = document.getElementById(str).innerHTML;
    document.getElementById("msgDiv").appendChild(txt);

}
function AddFriendSubmit(UID) {
    
   
    var param = document.getElementById("MessageText").value;

    $.post("/UserHome/Friend/AddGoodFriend", { GoodUserId: UID, Remark: param }, function (e) {
        if (e == "True") {
            alert("您的好友添加请求已经发送成功，正在等待对方的确认。");

            $.post("/UserHome/Friend/ToTask", null, function (e) {
                if (e != "") {
                    alert(e);
                  
                }
            });
            document.getElementById("msgTitle").onclick();
            return;
        }
        if (e == "False") {
            alert("用户昵称设置了隐私设置，不能发送好友申请");
            return;
        }
    });
}
function AddFriend(value) {
    $.post("/UserHome/Friend/JudgeFriend", { FriendUid: value }, function (e) {
        if (e == "False") {
            alert("不能加自己为好友哦");
            return;
        }
        $.post("/UserHome/Friend/_AddFriendDiv", { UId: value }, function (e) {
            sAlert(e);
        });
    });
   
}
function Close() {
    document.getElementById("msgTitle").onclick();
}

function keyup22() {
    var str = $("#MessageText").val();
    var len = $("#MessageText").val().length;
    $(".ceng_right_sz_curr").html(len);
    if (len > 50) {
        $("#MessageText").val(str.substring(0, 50));
        $(".ceng_right_sz_curr").html(50);
    }
}