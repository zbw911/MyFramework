//完成任务的弹出层
function OpenCompelteTask(str, wid, hei) {
    var msgw, msgh, bordercolor;
    msgw = 280; //提示窗口的宽度
    msgh = 66; //提示窗口的高度
    if (wid != "") {
        msgw = wid;
    }
    if (hei != "") {
        msgh = hei;
    }
    bordercolor = "#C1C1C1"; //提示窗口的边框颜色
    bgcolor = "#239DDC";
    titlecolor = "#239DDC"; //提示窗口的标题颜色

    var sWidth, sHeight;
    sWidth = document.body.offsetWidth;
    sHeight = $(window).height();
    if (document.getElementById("TaskDiv") == null) {
        var bgObj = document.createElement("div");
        bgObj.setAttribute('id', 'TaskbgDiv');
        bgObj.style.position = "absolute";
        bgObj.style.top = "0";
        bgObj.style.background = "#777";
        bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75";
        bgObj.style.opacity = "0.6";
        bgObj.style.left = "0";
        bgObj.style.width = document.body.offsetWidth + "px";
        bgObj.style.height = document.body.offsetHeight + "px";
        bgObj.style.zIndex = 10001;
        document.body.appendChild(bgObj);
        var msgObj = document.createElement("div")
        msgObj.setAttribute("id", "TaskDiv");
        bgObj.setAttribute('name', 'TaskDiv');
        msgObj.setAttribute("align", "center");
        msgObj.style.position = "absolute";
        msgObj.style.background = "white";
        msgObj.style.font = "12px/1.6em Verdana, Geneva, Arial, Helvetica, sans-serif";
        msgObj.style.border = "2px solid " + bordercolor;
        msgObj.style.width = msgw + "px";
        msgObj.style.minHeight = msgh + "px";
        msgObj.style.height = "	auto";
        msgObj.style.zoom = 1;
        msgObj.style.zIndex = 10002;
        msgObj.style.top = (document.documentElement.scrollTop + (sHeight - msgh) / 2) + "px";
        msgObj.style.left = (sWidth - msgw) / 2 + "px";

        var title = document.createElement("h4");
        title.style.display = "none";
        title.setAttribute("id", "TaskTitle");
        title.style.cursor = "pointer";
        title.style.position = "absolute";
        title.style.width = "14px";
        title.style.height = "14px";
        title.className = "alertdiv"
        title.innerHTML = '<span></span>';
        title.onclick = function () {
            document.body.removeChild(bgObj);
            document.getElementById("TaskDiv").removeChild(title);
            document.body.removeChild(msgObj);
        }
        document.body.appendChild(msgObj);
        document.getElementById("TaskDiv").appendChild(title);
    }
    var txt = document.createElement("div");
    txt.setAttribute("id", "TaskTxt");
    txt.innerHTML = str;
    document.getElementById("TaskDiv").appendChild(txt);
    /*
    if (document.getElementsByName("TaskDiv").length > 1);
    {
    var length = document.getElementsByName("TaskDiv").length;
       
    for (var i = 0; i < length; i++) {

    if (document.getElementsByName("TaskDiv")[i].innerHTML.length == 0)
    document.getElementsByName("TaskDiv")[i].tyle.display = "none";
    alert(document.getElementsByName("TaskDiv")[i].innerHTML);
    }
     
    }*/
}



function TaskCloseX() {
    document.getElementById("TaskTitle").onclick();
}

//完成任务调用的接口
function Task(i) {

    var taskdata = "";
    if (i == 1)//1 表示上传照片需要完成的任务
        taskdata = doTask("上传图片") + doTask("相册上传图片个数");
    if (i == 2)
        taskdata = doTask("寻找好友"); //+ doTask("好友数量")

}

function doTask(taskobject) {//完成任务接口{ async:false},
    //alert("dotask");
    $.ajax({
        type: "POST",
        url: "/userhome/Task/_TaskRewardsShow",
        async: false,
        data: "taskobject=" + taskobject,
        success: function (data) {
            if (data.length > 0) {
                var w = 280;
                var h = 66;
                OpenCompelteTask(data, w, h);
            }
        }
    });
}

function AddTaskCompNotice(taskobject) {
    $.ajax({
        type: "POST",
        url: "/userhome/Task/_AddTaskCompNotice",
        async: false,
        data: "taskobject=" + taskobject,
        success: function (data) {
            //成功后不进行任务操作。
        }
    });
}