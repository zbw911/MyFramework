var jSelectDate = {


    /**
    * 选项设置
    */
    settings: {
        css: "date",
        borderCss: "date",
        disabled: false,
        yearBegin: 1960,
        yearEnd: 2008,
        isShowLabel: true,
        showday: true
    },



    /**
    * 初始化对向
    * @param {Object} el 用于存放日期结果的文本框 jQuery DOM
    */
    init: function (els) {


        els.each(function () {


            var el = $(this);

            /* 取得旧的日期 */
            var elValue = el.val();
            elDate = new Date(elValue.split("-").join("/"));

            var nYear = elDate.getFullYear();
            var nMonth = jSelectDate.returnMonth(elDate.getMonth());
            var nDay = elDate.getDate();


            /* 隐藏给出的对向 */
            el.hide();

            /* 先算出当前共有多少个jSelectDate */
            var currentIdx = $("jSelectDateBorder").length + 1;

            /* 加入控件到文本框的位置 */
            var spanDate = document.createElement("span");
            spanDate.id = "spanDate" + currentIdx;
            spanDate.className = "jSelectDateBorder " + jSelectDate.settings.borderCss;
            spanDate.disabled = jSelectDate.settings.disabled;

            el.after(spanDate);

            /* 创建年 */
            var selYear = document.createElement("select");
            selYear.id = "selYear" + currentIdx
            selYear.className = jSelectDate.settings.css;
            selYear.disabled = jSelectDate.settings.disabled;

            /* 加入选项 */
            //alert(nYear + "-" + nMonth + "-" + nDay);
            if (nYear == "1900") {
                var option = document.createElement("option");
                option.value = "";
                option.innerHTML = "请选择";
                option.selected = true;
                selYear.appendChild(option);
                option = null;
            }

            for (var i = jSelectDate.settings.yearEnd; i >= jSelectDate.settings.yearBegin; i--) {
                var option = document.createElement("option");
                option.value = i;
                option.innerHTML = i;

                /* 判断是否有旧的值，如果有就选中 */
                if (!isNaN(nYear)) {
                    if (i == nYear) {
                        option.selected = true;
                    }
                }

                selYear.appendChild(option);
                option = null;

            }

            $(spanDate).append(selYear);

            /* 创建月 */
            var selMonth = document.createElement("select");
            selMonth.id = "selMonth" + currentIdx
            selMonth.className = jSelectDate.settings.css;
            selMonth.disabled = jSelectDate.settings.disabled;
            /* 加入选项 */
            if (nYear == "1900") {
                var option = document.createElement("option");
                option.value = "";
                option.innerHTML = "请选择";
                option.selected = true;
                selMonth.appendChild(option);
                option = null;
            }

            for (var i = 1; i <= 12; i++) {
                var option = document.createElement("option");
                option.value = i;
                option.innerHTML = i;

                /* 判断是否有旧的值，如果有就选中 */
                if (!isNaN(nMonth)) {
                    if (i == nMonth && nYear != "1900") {
                        option.selected = true;
                    }
                }

                selMonth.appendChild(option);
                option = null;
            }
            /* 加入控件到文本框的位置 */
            $(selYear).after(selMonth);


            /* 创建日 */
            var selDay = document.createElement("select");
            selDay.id = "selDay" + currentIdx
            selDay.className = jSelectDate.settings.css;
            selDay.disabled = jSelectDate.settings.disabled;
            if (!jSelectDate.settings.showday) {
                $(selDay).hide();
            }

            /* 算出最大的天数 */
            var maxDayNum = 30;
            if (nMonth == 2) {
                if (jSelectDate.isLeapYear(nYear)) {
                    maxDayNum = 29;
                }
                else {
                    maxDayNum = 28;
                }
            }
            else if (jSelectDate.isLargeMonth(nMonth)) {
                maxDayNum = 31;
            }

            /* 加入选项 */
            if (nYear == "1900") {
                var option = document.createElement("option");
                option.value = "";
                option.innerHTML = "请选择";
                option.selected = true;
                selDay.appendChild(option);
                option = null;
            }

            for (var i = 1; i <= maxDayNum; i++) {

                var option = document.createElement("option");
                option.value = i;
                option.innerHTML = i;

                /* 判断是否有旧的值，如果有就选中 */
                if (!isNaN(nDay)) {
                    if (i == nDay && nYear != "1900") {
                        option.selected = true;
                    }
                }

                selDay.appendChild(option);
                option = null;
            }
            /* 加入控件到文本框的位置 */
            $(selMonth).after(selDay);

            if (jSelectDate.settings.isShowLabel) {
                $(selMonth).before(" 年 ");
                $(selDay).before(" 月 ");
                if (jSelectDate.settings.showday) {
                    $(selDay).after(" 日");
                }
            } else {
                $(selMonth).before(" ");
                $(selDay).before(" ");
            }

            /* 返回当前选择的日期 */
            var getDate = function () {
                var year = $(selYear).val();
                var month = $(selMonth).val();
                var day = $(selDay).val();
                el.val(year + "-" + month + "-" + day);
            }

            function GetBirthday() {
                var birth = $(selYear).val() + $(selMonth).val() + $(selDay).val();
                if (birth != "" && birth.length > 5) {
                    if ($(selMonth).val().length == 1) {
                        birth = $(selYear).val() + "-0" + $(selMonth).val();
                    }
                    else {
                        birth = $(selYear).val() + "-" + $(selMonth).val();
                    }
                    if ($(selDay).val().length == 1) {
                        birth = birth + "-0" + $(selDay).val();
                    }
                    else {
                        birth = birth + "-" + $(selDay).val();
                    }
                    $.ajax({
                        type: "POST",
                        url: "/RegNav/GetXingZuo",
                        data: { "birthday": birth },
                        async: true,
                        success: function (data) {
                            $("#constellation").val(data);
                        }
                    });
                }
            }
            /**
            * 给几个下拉列表加入更改后的事件
            */
            $(selDay).change(function () {
                GetBirthday();
                return getDate();
            });
            $(selMonth).change(function () {

                jSelectDate.progressDaySize(this, true);
                GetBirthday();
                /* 更新文本框中的日期 */
                return getDate();
            });
            $(selYear).change(function () {
                jSelectDate.progressDaySize(this, false);
                return getDate();
            });
        })


    },

    /**
    * 判断是否闰年
    * @param {Object} year
    * @author 没剑 http://regedit.cnblogs.com
    */
    isLeapYear: function (year) {
        return (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0)));
    },

    /**
    * 判断是否是大月
    * @param {Object} monthNum
    */
    isLargeMonth: function (monthNum) {
        var largeArray = [true, false, true, false, true, false, true, true, false, true, false, true];
        return largeArray[monthNum - 1];
    },

    returnMonth: function (num) {
        var arr = new Array("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12");
        return arr[num];
    },

    /**
    * 创建一个Option对象
    * @param {Object} value 值
    * @param {Object} text 文本
    */
    createOption: function (value, text) {
        var option = document.createElement("option");
        option.value = value;
        option.innerHTML = text;
        return option;
    },

    /**
    * 处理天数
    * @param {Object} el 下拉列表对像
    * @param {Object} isMonth 是否是月的下拉列表 或者就处理 年的下拉列表
    */
    progressDaySize: function (el, isMonth) {
        if (isMonth == true) {
            /* 选择月时处理大月、小月和二月的情况 */
            var month = $(el).val();
            var year = $($("select", $(el).parent())[0]).val()
            var selDay = $($("select", $(el).parent())[2]);
            if (month == 2) {

                /* 2月处理 */
                $("option:contains('31')", selDay).remove();
                $("option:contains('30')", selDay).remove();

                /* 闰年处理 */
                if (!jSelectDate.isLeapYear(year)) {
                    $("option:contains('29')", selDay).remove();
                }
                else {

                    if ($("option:contains('29')", selDay).length == 0) {
                        selDay.append(jSelectDate.createOption(29, 29));
                    }
                }
            }
            else
                if (!jSelectDate.isLargeMonth(month)) {

                    /* 小月处理 */
                    if ($("option:contains('30')", selDay).length == 0) {
                        selDay.append(jSelectDate.createOption(30, 30));
                    }

                    $("option:contains('31')", selDay).remove();
                }
                else {

                    /* 大月处理 */
                    if ($("option:contains('30')", selDay).length == 0) {
                        selDay.append(jSelectDate.createOption(30, 30));
                    }

                    if ($("option:contains('31')", selDay).length == 0) {
                        selDay.append(jSelectDate.createOption(31, 31));
                    }
                }
        }
        else {
            /* 处理闰年的二月问题 */
            var panelDate = $(el).parent();
            var year = $(el).val();
            var month = $($("select", panelDate)[1]).val()
            var selDay = $($("select", panelDate)[2]);
            if (month == 2) {
                $("option:contains('31')", selDay).remove();
                $("option:contains('30')", selDay).remove();
                if (!jSelectDate.isLeapYear(year)) {
                    $("option:contains('29')", selDay).remove();
                }
                else {

                    if ($("option:contains('29')", selDay).length == 0) {
                        selDay.append(jSelectDate.createOption(29, 29));
                    }
                }
            }

        }
    }

}

jQuery.fn.jSelectDate = function (settings) {

    var getNowYear = function () {
        /* 得到现在的年 */
        var date = new Date();
        return date.getFullYear();
    }

    jSelectDate.settings.yearEnd = getNowYear();

    $.extend(jSelectDate.settings, settings);


    jSelectDate.init($(this));

    return $(this);

}
