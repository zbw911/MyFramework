function GetCharLength(str) {
    var iLength = 0;
    for (var i = 0; i < str.length; i++) {
        if (str.charCodeAt(i) > 255) {
            iLength += 2;
        } else {
            iLength += 1;
        }
    }
    return iLength;
}
function CutStr(Str, Len) {
    var CurStr = "";
    for (var i = 0; i < Str.length; i++) {
        CurStr += Str.charAt(i);
        if (GetCharLength(CurStr) > Len) {
            return Str.substring(0, i);
        }
    }
    return CurStr;
}

function Fold() {
    $(".d").toggle(function () {
    });
}

//添加收藏
function addfavorite(url, title) {
    if (document.all) {
        window.external.addFavorite(url, title);
    }
    else if (window.sidebar) {
        window.sidebar.addPanel(title, url, "");
    }
}

//复制
function copyCode(txt) {
    if (!!copy2Clipboard(txt) == true) {
        alert("生成的代码已经复制到粘贴板，你可以使用Ctrl+V 贴到需要的地方去了哦！  ");
    }
}
copy2Clipboard = function (txt) {
    if (window.clipboardData) {
        window.clipboardData.clearData();
        return window.clipboardData.setData("Text", txt);
    }
    else if (navigator.userAgent.indexOf("Opera") != -1) {
        window.location = txt;
    }
    else if (window.netscape) {
        try {
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
        }
        catch (e) {
            alert("您的firefox安全限制限制您进行剪贴板操作，请打开’about:config’将signed.applets.codebase_principal_support’设置为true’之后重试，相对路径为firefox根目录/greprefs/all.js");
            return false;
        }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip) return false;
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans) return false;
        trans.addDataFlavor('text/unicode');
        var str = new Object();
        var len = new Object();
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
        var copytext = txt; str.data = copytext;
        trans.setTransferData("text/unicode", str, copytext.length * 2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip) return false;
        clip.setData(trans, null, clipid.kGlobalClipboard);
        return true;
    }
}

var tab_game_type = 0;
//游戏列表(注:navid)
function showgamelist(navid) {
    tabChange($("#" + navid), 0);
}
//平台数据列表
function showgameplatlist() {
    var gameid = $("#gameid").val()
    if (!!gameid == false) return;
    else {
        getPlatList(gameid);
    }
}
//服务器列表
function showserverlist() {
    var gameid = $("#gameid").val()
    var platid = $("#platid").val();
    if (!!gameid == false || !!platid == false) return;
    else {
        getServerList(gameid, platid);
    }
}
//字母筛选方法
function getGameABC(url, shortname, divid) {
    $.post(url,
        {
            action: 'gamelist',
            shortname: shortname,
            game_class: tab_game_type
        }, function (data) {
            $('#' + divid).html(data);
        });
    //$(".ain").removeClass("ain");
    //$(e).addClass("ain");
}
//服务器数据
function getServerList(url, gameid, platid, divid) {
    $.post(url,
        {
            action: 'getServer',
            gameid: gameid,
            platid: platid,
            game_type: 0,
            shortname: ''

        }, function (data) {
            $("#" + divid).html(data);
        });
}
//游戏平台数据
function getPlatList(url, gameid, divid) {
    var gameid = (gameid > 0) ? gameid : 0;
    $.post(url,
        {
            action: 'getPlat',
            gameid: gameid,
            game_type: 0,
            shortname: ''
        }, function (data) {
            $("#" + divid).html(data);
        });
}

//得到服务器id
function setServer(serverid) {

}
//得到平台id
function setplat(platid) {
}

//游戏id
function setgame(gameid, gamename) {
}

function tabChange(type, letterdiv) {
    tab_game_type = type;
    //$(".spanin").removeClass("spanin");
    //$(e).addClass("spanin");
    getGameABC($('#' + letterdiv), 'A,B,C,D');
}

//加入游戏团
function joinGroup(uid, gid, joinPerm, eid) {
    var myAjax = $.post(
        "/GameGroup/Group/joinGroup",
        {
            uid: uid,
            gId: gid,
            joinPerm: joinPerm
        }, function (d) {
            if ('0' == d) {
                alert("您还没有登录，请登录后再申请");
            }
            else if ('1' == d) {
                $("#" + eid).html("<a href='javascript:void(0)' class='game_add'><img src='/content/gametuan/images/adding.gif'/></a>");
            }
            else if ('2' == d) {
                $("#" + eid).html("<a href='javascript:void(0)' onclick=\"outGroup('" + uid + "','" + gid + "','join'," + joinPerm + ")\" class='game_add'><img src='/content/gametuan/images/out.gif'/></a>");
                doTask("寻找组织");
            }
            else {
                $("#" + eid).html("<a href='javascript:void(0)' onclick=\"joinGroup('" + uid + "','" + gid + "','" + joinPerm + "','join')\" class='game_add'><img src='/content/gametuan/images/add_botton.gif'/></a>");
            }
        });
}

//退出游戏团
function outGroup(uid, gid, eid, joinPerm) {
    var myAjax = $.post(
        "/GameGroup/Group/outGroup",
        {
            uid: uid,
            gId: gid
        }, function (d) {
            if ('1' == d) {
                $("#" + eid).html("<a href='javascript:void(0)' onclick=\"joinGroup('" + uid + "','" + gid + "','" + joinPerm + "','join')\" class='game_add'><img src='/content/gametuan/images/add_botton.gif'/></a>");
            }
        });
}
