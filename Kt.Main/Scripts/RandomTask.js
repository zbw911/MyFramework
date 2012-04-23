/* AddBy 赵灵军 2011-06-02
*  随机任务相关的方法
*/



//随机任务的弹出层
function _OpenRandomTask(str) {
    //检测当前页面中是否有其他的弹出层，若有的话，就返回一个空，这样的话，就不会存在弹出层相互盖住的情况
    //zlj 2011-06-08 明天来了需要测试一下，看是否可行。
    if (document.getElementById("bgDiv") != null || document.getElementById("TaskDiv") != null)
        return;

    var msgw, msgh, bordercolor;
    msgw = 450; //提示窗口的宽度
    msgh = 225; //提示窗口的高度
    bordercolor = "#C1C1C1"; //提示窗口的边框颜色
    bgcolor = "#239DDC";
    titlecolor = "#239DDC"; //提示窗口的标题颜色

    var sWidth, sHeight;
    sWidth = document.body.offsetWidth;
    sHeight = $(window).height();
    var bgObj = document.createElement("div");
    bgObj.setAttribute('id', '__TaskRandombgDiv');
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
    msgObj.setAttribute("id", "__TaskRandommsgDiv");
    msgObj.setAttribute("align", "center");
    msgObj.style.position = "absolute";
    msgObj.style.background = "white";
    msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
    msgObj.style.border = "2px solid " + bordercolor;
    msgObj.style.width = msgw + "px";
    msgObj.style.paddingBottom = "20px"


    msgObj.height < msgh ? msgh : "auto";
    msgObj.style.zoom = 1;
    msgObj.style.zIndex = 100002;
    msgObj.style.top = (document.documentElement.scrollTop + (sHeight - msgh) / 2) + "px";
    msgObj.style.left = (sWidth - msgw) / 2 + "px";
    var title = document.createElement("h4");
    title.setAttribute("id", "__TaskRandommsgTitle");
    title.style.cursor = "pointer";
    title.style.position = "absolute";
    title.style.width = "14px";
    title.style.height = "14px";
    title.className = "alertdiv"
    title.innerHTML = '<span></span>';
    title.onclick = function () {
        document.body.removeChild(bgObj);
        document.getElementById("__TaskRandommsgDiv").removeChild(title);
        document.body.removeChild(msgObj);
    }
    document.body.appendChild(msgObj);
    document.getElementById("__TaskRandommsgDiv").appendChild(title);
    var txt = document.createElement("div");
    txt.setAttribute("id", "__TaskRandommsgTxt");
    txt.innerHTML = str;
    document.getElementById("__TaskRandommsgDiv").appendChild(txt);
    //初始化弹出层的状态
    //_InitTaskBtnState();
}

//随机任务的关闭弹出层函数
function _CloseRandomTaskX() {
    document.getElementById("__TaskRandommsgTitle").onclick();
}

//随机任务 按钮初始化
function _InitTaskBtnState() {
    var state = $("#radomtxt_state").val();
    if (state != "" && state != "undefined") {
        if (state == 0)
            $("div[class='task_go']").append("<a href='javascript:void(0)' class='task_goto' onclick='javascript:AcceptRadomTask();return false;'>接受任务</a>"); //href属性不能换成# 否则弹出alert就到了也页面的上面
        else
            if (state == 1) {
                var id = $("#radomtxt_id").val();
                $("div[class='task_go']").append("<a href='javascript:void(0)' class='task_goto' onclick='javascript:CompeleteRadomTask();return false;'>去做任务</a>");
            }
            else
                $("div[class='task_go']").append("已完成");

    }
}

//拒绝接收任务
function RefuseRadomTask() {
    var id = $("#radomtxt_id").val();
    $.ajax({
        type: "POST",
        url: "/userhome/task/_RefusedTask",
        async: false,
        data: "taskid=" + id,
        success: function (e) {
            if (e.toLowerCase() == "true") {
                alert("您已经拒绝接收此任务");
                _CloseRandomTaskX();
            }
        }
    });
}

//接收任务函数
function AcceptRadomTask() {
    var id = $("#radomtxt_id").val();    
    $.post("/userhome/task/AcceptTask", { taskid: id }, function (e) {
        if (e.toLowerCase() == "true") {
            alert("接受任务成功");
            $("#AcceptBtn").text("去做任务");
            $("#AcceptBtn").removeAttr("onclick");
            $("#AcceptBtn").bind('click', function () { CompeleteRadomTask(); return false; });
        }
    }
    )
}
//任务引导函数
function CompeleteRadomTask() {
    var url = $("#radomtxt_url").val();    
    if (url == undefined || url == null || url.length == 0)
        alert("现在此模块没有链接，请通知赵灵军加上！");
    else {
        if (url.indexOf("http://") > -1) {
            window.open(url);
            _CloseRandomTaskX();
        }
        else
            window.location.href = url;
    }
}