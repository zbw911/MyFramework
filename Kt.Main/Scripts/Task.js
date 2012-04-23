function CompeleteTask(taskid) {    //任务引导函数
    var url = "";
    if (taskid != undefined)
        url = $("input[id='txtydurl+" + taskid + "']").val();

    if (url == undefined || url == null || url.length == 0)
        alert("现在此模块没有链接，请通知赵灵军加上！");
    else {
        if (url.indexOf("http://") > -1) {
            window.open(url);
            //关闭弹出层
            WinClose();
        }
        else
            window.location.href = url;
    }
}
//任务模块 初始化数据
function OnLoadTask() {
    var ttype = 0;
    var temp = $("li[class='up_nav_index']").first().attr("id");
    if (temp != null) {
        temp = temp.substr(temp.indexOf("_") + 1, temp.length - temp.indexOf("_") - 1);
        ttype = temp;
        $.post("/userhome/task/_AllTask", { tasktype: ttype }, function (e) {
            $("#tasklist").html(e);
            //全部任务 标签样式
            var count = $("div[id^='uctask+']").length;
            for (var i = 0; i < count; i++) {//必须是i+1，原因是任务类型id是从1开始的
                $("div[id='uctask+" + (i+1) + "'] ul[class='uc_task_list_wapper'] li:odd").addClass("uc_task_list_odd");
            }
            if (count == 0)//进行中 完成 任务标签验证
                $("ul[class='uc_task_list_wapper'] li:odd").addClass("uc_task_list_odd");
        });
    }
}
/*以下是 接收任务弹出层用到的函数*/
function InitBtnSpan() {
    var state = $("#txt_state").val();
    //alert(state);
    if (state != "" && state != "undefined") {
        if (state == 0)
            $("div[class='task_go']").append("<a href='javascript:void(0)' class='task_goto' onclick='javascript:AcceptTask();return false;'>接受任务</a>"); //onclick后面的return false不能去掉，原因：去掉后ie6就不支持了
        else
            if (state == 1) {
                var id = $("#txt_id").val();
                //alert(id);
                $("div[class='task_go']").append("<a href='javascript:void(0)' class='task_goto' onclick='javascript:CompeleteTask(" + id + ");return false;'>去做任务</a>");
            }
            else
                $("div[class='task_go']").append("已完成");
    }
}
function AcceptTask() {
    var id = $("#txt_id").val();
    $.post("/userhome/task/AcceptTask", { taskid: id }, function (e) {
        if (e.toLowerCase() == "true") {
            alert("接受任务成功");
            if ($("p[id='test+" + id + "']") != null)
                $("p[id='test+" + id + "']").html("<a href='javascript:void(0)' onclick=\"javascript:CompeleteTask(" + id + ");return false;\">去做任务</a>");
            $("a[class='task_goto']").text("去做任务");
            $("a[class='task_goto']").removeAttr("onclick");
            $("a[class='task_goto']").bind('click', function () { CompeleteTask(id); return false; });
            
        }
    }
        )
}
function WinClose() {
    var obj = document.getElementById("msgTitle");
    if (obj != null && obj != undefined) {
        document.getElementById("msgTitle").onclick();
        OnLoadTask();
    }
}
