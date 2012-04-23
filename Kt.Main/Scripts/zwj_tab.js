

function ShowTab(o) {
    var obj = $('div.zwj_serch_game_xinxi');
    for (var i = 0; i < obj.length; i++) {
        $(obj).eq(i).hide();
    }
    $("#a" + o).show();

    if (o == "1") { $("#type").val("game"); $("#tagname").hide(); }
    else if (o == "2") { $("#type").val("role"); $("#tagname").hide(); }
    else if (o == "3") { $("#type").val("nickname"); $("#tagname").hide(); }
    else if (o == "4") { $("#type").val("usertag"); $("#tagname").show(); }

}
function ShowTab2(o) {
    var obj = $('.zwj_right-tab');
    for (var i = 0; i < obj.length; i++) {
        $(obj).eq(i).hide();
    }
    $("#a" + o).show();
}

