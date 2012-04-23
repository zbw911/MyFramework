jQuery.fn.boxy = function (options) {
    options = options || {};
    return this.each(function () {
        var node = this.nodeName.toLowerCase(), self = this;
        if (node == 'a') {
            jQuery(this).click(function () {
                var active = Boxy.linkedTo(this),
                    href = this.getAttribute('href'),
                    localOptions = jQuery.extend({ actuator: this, title: this.title }, options);

                if (active) {
                    active.show();
                } else if (href.indexOf('#') >= 0) {
                    var content = jQuery(href.substr(href.indexOf('#'))),
                        newContent = content.clone(true);
                    content.remove();
                    localOptions.unloadOnHide = false;
                    new Boxy(newContent, localOptions);
                } else {
                    if (!localOptions.cache) localOptions.unloadOnHide = true;
                    Boxy.load(this.href, localOptions);
                }

                return false;
            });
        } else if (node == 'form') {
            jQuery(this).bind('submit.boxy', function () {
                Boxy.confirm(options.message || 'Please confirm:', function () {
                    jQuery(self).unbind('submit.boxy').submit();
                });
                return false;
            });
        }
    });
};

function Boxy(element, options) {

    this.boxy = jQuery(Boxy.WRAPPER);
    jQuery.data(this.boxy[0], 'boxy', this);

    this.visible = false;
    this.options = jQuery.extend({}, Boxy.DEFAULTS, options || {});

    if (this.options.modal) {
        this.options = jQuery.extend(this.options, { center: true, draggable: true });
    }

    if (this.options.actuator) {
        jQuery.data(this.options.actuator, 'active.boxy', this);
    }

    this.setContent(element || "<div></div>");
    this._setupTitleBar();

    this.boxy.css('display', 'none').appendTo(document.body);
    this.toTop();

    if (this.options.fixed) {
        if (jQuery.browser.msie && jQuery.browser.version < 7) {
            this.options.fixed = false;
        } else {
            this.boxy.addClass('fixed');
        }
    }

    if (this.options.center && Boxy._u(this.options.x, this.options.y)) {
        this.center();
    } else {
        this.moveTo(
            Boxy._u(this.options.x) ? this.options.x : Boxy.DEFAULT_X,
            Boxy._u(this.options.y) ? this.options.y : Boxy.DEFAULT_Y
        );
    }

    if (this.options.show) this.show();

};

Boxy.EF = function () { };

jQuery.extend(Boxy, {

    WRAPPER: "<table cellspacing='0' cellpadding='0' border='0' class='boxy-wrapper'>" +
                "<tr><td class='top-left'></td><td class='top_boxy'></td><td class='top-right'></td></tr>" +
                "<tr><td class='left_boxy'></td><td class='boxy-inner'></td><td class='right_boxy'></td></tr>" +
                "<tr><td class='bottom-left'></td><td class='bottom_boxy'></td><td class='bottom-right'></td></tr>" +
                "</table>",

    DEFAULTS: {
        title: null,           // titlebar text. titlebar will not be visible if not set.
        closeable: true,           // display close link in titlebar?
        draggable: true,           // can this dialog be dragged?
        clone: false,          // clone content prior to insertion into dialog?
        actuator: null,           // element which opened this dialog
        center: true,           // center dialog in viewport?
        show: true,           // show dialog immediately?
        modal: false,          // make dialog modal?
        fixed: true,           // use fixed positioning, if supported? absolute positioning used otherwise
        closeText: '',      		// text to use for default close link,now it's a picture
        unloadOnHide: false,          // should this dialog be removed from the DOM after being hidden?
        clickToFront: false,          // bring dialog to foreground on any click (not just titlebar)?
        behaviours: Boxy.EF,        // function used to apply behaviours to all content embedded in dialog.
        afterDrop: Boxy.EF,        // callback fired after dialog is dropped. executes in context of Boxy instance.
        afterShow: Boxy.EF,        // callback fired after dialog becomes visible. executes in context of Boxy instance.
        afterHide: Boxy.EF,        // callback fired after dialog is hidden. executed in context of Boxy instance.
        beforeUnload: Boxy.EF         // callback fired after dialog is unloaded. executed in context of Boxy instance.
    },

    DEFAULT_X: 50,
    DEFAULT_Y: 50,
    zIndex: 1337,
    dragConfigured: false,
    resizeConfigured: false,
    dragging: null,

    load: function (url, options) {
        options = options || {};
        var baseTitle = options.title;
        var obj = new Boxy("<table cellspacing='0' cellpadding='0' border='0'><tr><td style='width:300px;height:120px;text-align:center;' valign='middle'><img src='/Content/default/images/loading2.gif' border='0' /></td></tr></table>", options);
        obj.setTitle('加载中,请稍候...');
        var ajax = {
            url: url, type: 'GET', dataType: 'html', cache: false, success: function (html) {
                html = jQuery(html);
                if (options.filter) html = jQuery(options.filter, html);
                obj.setContent(html);
                obj.center();
                obj._fire('afterShow');
                obj.setTitle(baseTitle);
            }
        };
        jQuery.each(['type', 'cache'], function () {
            if (this in options) {
                ajax[this] = options[this];
                delete options[this];
            }
        });
        jQuery.ajax(ajax);

    },

    get: function (ele) {
        var p = jQuery(ele).parents('.boxy-wrapper');
        return p.length ? jQuery.data(p[0], 'boxy') : null;
    },

    linkedTo: function (ele) {
        return jQuery.data(ele, 'active.boxy');
    },

    alert: function (message, callback, options) {
        return Boxy.ask(message, ['确 认'], callback, options);
    },

    confirm: function (message, after, options) {
        return Boxy.ask(message, ['确 认', '取 消'], function (response) {
            if (response == '确 认') after();
        }, options);
    },
    fullConfirm: function (message, after, cancel, options) {
        return Boxy.ask(message, ['确 认', '取 消'], function (response) {
            if (response == '确 认') after();
            if (response == '取 消') cancel();
        }, options);
    },

    delconfirm: function (after, input, bb) {
        input = (!!input) ? input : 'DELETE';
        bb = (!!bb) ? bb : '请输入 "<span style="color:#B70000;font-weight:bold;">' + input + '</span>" 以确认删除！';
        cc = '<br /><input type="text" id="boxyDelText" style="border:1px solid #ccc;width:150px;margin-bottom:15px;_margin-bottom:0px;" />';
        return Boxy.ask(bb + cc, ['确 认', '取 消'], function (response) {
            if (response == '确 认') {
                if ($('#boxyDelText').val() == input) {
                    after();
                } else {
                    Boxy.delconfirm(after, input, '输入 "<span style="color:#B70000;font-weight:bold;">' + input + '</span>" 错误，请注意大小写！');
                }
            }
        }, { modal: false, title: '请确认您的操作', unloadOnHide: true });
    },

    flt: function (obj, after, ts) {
        ts = (!!ts) ? ts : 50;
        if (ts < 96) {
            $('#' + obj).css("background", "rgb(100%,100%," + ts + "%)");
            ts += 4;
            setTimeout(function () { Boxy.flt(obj, after, ts); }, 100);
        } else {
            $('#' + obj).css("background", "rgb(100%,100%," + ts + "%)").fadeOut();
            setTimeout(function () { after(); }, 1000);
        }
    },

    ask: function (question, answers, callback, options) {

        options = jQuery.extend({ modal: true, closeable: false },
                                options || {},
                                { show: true, unloadOnHide: true });

        var body = jQuery('<div></div>').append(jQuery('<div class="question"></div>').html(question));

        // ick
        var map = {}, answerStrings = [];
        if (answers instanceof Array) {
            for (var i = 0; i < answers.length; i++) {
                map[answers[i]] = answers[i];
                answerStrings.push(answers[i]);
            }
        } else {
            for (var k in answers) {
                map[answers[k]] = k;
                answerStrings.push(answers[k]);
            }
        }

        var buttons = jQuery('<form class="boxy_bottom_bar"></form>');
        buttons.html(jQuery.map(answerStrings, function (v) {
            if (v == '确 认') return "<input type='button' id='btnOk' value='" + v + "' />";
            if (v == '取 消') return "<input type='button' id='btnCancel' value='" + v + "' />";
        }).join(' '));

        jQuery('input[type=button]', buttons).click(function () {
            var clicked = this;
            Boxy.get(this).hide(function () {
                if (callback) callback(map[clicked.value]);
            });
        });

        body.append(buttons);
        new Boxy(body, options);
    },

    isModalVisible: function () {
        return jQuery('.boxy-modal-blackout').length > 0;
    },

    _u: function () {
        for (var i = 0; i < arguments.length; i++)
            if (typeof arguments[i] != 'undefined') return false;
        return true;
    },

    _handleResize: function (evt) {
        var d = jQuery(document);
        jQuery('.boxy-modal-blackout').css('display', 'none').css({
            width: d.width(), height: d.height()
        }).css('display', 'block');
    },

    _handleDrag: function (evt) {
        var d;
        if (d = Boxy.dragging) {
            d[0].boxy.css({ left: evt.pageX - d[1], top: evt.pageY - d[2] });
        }
    },

    _nextZ: function () {
        return Boxy.zIndex++;
    },

    _viewport: function () {
        var d = document.documentElement, b = document.body, w = window;
        return jQuery.extend(
            jQuery.browser.msie ?
                { left: b.scrollLeft || d.scrollLeft, top: b.scrollTop || d.scrollTop} :
                { left: w.pageXOffset, top: w.pageYOffset },
            !Boxy._u(w.innerWidth) ?
                { width: w.innerWidth, height: w.innerHeight} :
                (!Boxy._u(d) && !Boxy._u(d.clientWidth) && d.clientWidth != 0 ?
                    { width: d.clientWidth, height: d.clientHeight} :
                    { width: b.clientWidth, height: b.clientHeight }));
    }

});

Boxy.prototype = {
    estimateSize: function () {
        this.boxy.css({ visibility: 'hidden', display: 'block' });
        var dims = this.getSize();
        this.boxy.css('display', 'none').css('visibility', 'visible');
        return dims;
    },

    getSize: function () {
        return [this.boxy.width(), this.boxy.height()];
    },

    getContentSize: function () {
        var c = this.getContent();
        return [c.width(), c.height()];
    },

    getPosition: function () {
        var b = this.boxy[0];
        return [b.offsetLeft, b.offsetTop];
    },

    getCenter: function () {
        var p = this.getPosition();
        var s = this.getSize();
        return [Math.floor(p[0] + s[0] / 2), Math.floor(p[1] + s[1] / 2)];
    },

    getInner: function () {
        return jQuery('.boxy-inner', this.boxy);
    },

    getContent: function () {
        return jQuery('.boxy-content', this.boxy);
    },

    setContent: function (newContent) {
        newContent = jQuery(newContent).css({ display: 'block' }).addClass('boxy-content');
        if (this.options.clone) newContent = newContent.clone(true);
        this.getContent().remove();
        this.getInner().append(newContent);
        this._setupDefaultBehaviours(newContent);
        this.options.behaviours.call(this, newContent);
        return this;
    },

    moveTo: function (x, y) {
        this.moveToX(x).moveToY(y);
        return this;
    },

    moveToX: function (x) {
        if (typeof x == 'number') this.boxy.css({ left: x });
        else this.centerX();
        return this;
    },

    moveToY: function (y) {
        if (typeof y == 'number') this.boxy.css({ top: y });
        else this.centerY();
        return this;
    },

    centerAt: function (x, y) {
        var s = this[this.visible ? 'getSize' : 'estimateSize']();
        if (typeof x == 'number') this.moveToX(x - s[0] / 2);
        if (typeof y == 'number') this.moveToY(y - s[1] / 2);
        return this;
    },

    centerAtX: function (x) {
        return this.centerAt(x, null);
    },

    centerAtY: function (y) {
        return this.centerAt(null, y);
    },

    center: function (axis) {
        var v = Boxy._viewport();
        var o = this.options.fixed ? [0, 0] : [v.left, v.top];
        if (!axis || axis == 'x') this.centerAt(o[0] + v.width / 2, null);
        if (!axis || axis == 'y') this.centerAt(null, o[1] + v.height / 2);
        return this;
    },

    centerX: function () {
        return this.center('x');
    },

    centerY: function () {
        return this.center('y');
    },

    resize: function (width, height, after) {
        if (!this.visible) return;
        var bounds = this._getBoundsForResize(width, height);
        this.boxy.css({ left: bounds[0], top: bounds[1] });
        this.getContent().css({ width: bounds[2], height: bounds[3] });
        if (after) after(this);
        return this;
    },

    tween: function (width, height, after) {
        if (!this.visible) return;
        var bounds = this._getBoundsForResize(width, height);
        var self = this;
        this.boxy.stop().animate({ left: bounds[0], top: bounds[1] });
        this.getContent().stop().animate({ width: bounds[2], height: bounds[3] }, function () {
            if (after) after(self);
        });
        return this;
    },

    isVisible: function () {
        return this.visible;
    },

    show: function () {
        if (this.visible) return;
        if (this.options.modal) {
            var self = this;
            if (!Boxy.resizeConfigured) {
                Boxy.resizeConfigured = true;
                jQuery(window).resize(function () { Boxy._handleResize(); });
            }
            this.modalBlackout = jQuery('<div class="boxy-modal-blackout"></div>')
                .css({ zIndex: Boxy._nextZ(),
                    opacity: 0.2,
                    width: jQuery(document).width(),
                    height: jQuery(document).height()
                })
                .appendTo(document.body);
            this.toTop();
            if (this.options.closeable) {
                jQuery(document.body).bind('keypress.boxy', function (evt) {
                    var key = evt.which || evt.keyCode;
                    if (key == 27) {
                        self.hide();
                        jQuery(document.body).unbind('keypress.boxy');
                    }
                });
            }
        }
        this.boxy.stop().css({ opacity: 1 }).show();
        this.visible = true;
        this._fire('afterShow');
        return this;
    },

    hide: function (after) {
        if (!this.visible) return;
        var self = this;
        if (this.options.modal) {
            jQuery(document.body).unbind('keypress.boxy');
            this.modalBlackout.animate({ opacity: 0 }, function () {
                jQuery(this).remove();
            });
        }
        this.boxy.stop().animate({ opacity: 0 }, 300, function () {
            self.boxy.css({ display: 'none' });
            self.visible = false;
            self._fire('afterHide');
            if (after) after(self);
            if (self.options.unloadOnHide) self.unload();
        });
        return this;
    },

    toggle: function () {
        this[this.visible ? 'hide' : 'show']();
        return this;
    },

    hideAndUnload: function (after) {
        this.options.unloadOnHide = true;
        this.hide(after);
        return this;
    },

    unload: function () {
        this._fire('beforeUnload');
        this.boxy.remove();
        if (this.options.actuator) {
            jQuery.data(this.options.actuator, 'active.boxy', false);
        }
    },

    toTop: function () {
        this.boxy.css({ zIndex: Boxy._nextZ() });
        return this;
    },

    getTitle: function () {
        return jQuery('> .title-bar h2', this.getInner()).html();
    },

    setTitle: function (t) {
        jQuery('> .title-bar h2', this.getInner()).html(t);
        return this;
    },


    _getBoundsForResize: function (width, height) {
        var csize = this.getContentSize();
        var delta = [width - csize[0], height - csize[1]];
        var p = this.getPosition();
        return [Math.max(p[0] - delta[0] / 2, 0),
                Math.max(p[1] - delta[1] / 2, 0), width, height];
    },

    _setupTitleBar: function () {
        if (this.options.title) {
            var self = this;
            if (this.options.titletype == 2) {
                var tb = jQuery("<div class='title-bar'></div>").css("background-color", "#ffffff").html("<div class='title_a'><h3>" + this.options.title + "</h3><span class='gb'><a href='javascript:void(0);' onclick='javascript:Boxy.get(this).hide();' ><img src='/Content/default/gamenav/images/gb.png' width='23' height='23' alt='关闭' border='0'/></a></span></div>");
            }
            else {
                var tb = jQuery("<div class='title-bar'></div>").html("<h2>" + this.options.title + "</h2>");
                if (this.options.closeable) {
                    tb.append(jQuery("<a href='javascript:void(0);' class='close'></a>").html(this.options.closeText));
                }
            }
            if (this.options.draggable) {
                tb[0].onselectstart = function () { return false; }
                tb[0].unselectable = 'on';
                tb[0].style.MozUserSelect = 'none';
                if (!Boxy.dragConfigured) {
                    jQuery(document).mousemove(Boxy._handleDrag);
                    Boxy.dragConfigured = true;
                }
                tb.mousedown(function (evt) {
                    self.toTop();
                    Boxy.dragging = [self, evt.pageX - self.boxy[0].offsetLeft, evt.pageY - self.boxy[0].offsetTop];
                    jQuery(this).addClass('dragging');
                }).mouseup(function () {
                    jQuery(this).removeClass('dragging');
                    Boxy.dragging = null;
                    self._fire('afterDrop');
                });
            }
            this.getInner().prepend(tb);
            this._setupDefaultBehaviours(tb);
        }
    },

    _setupDefaultBehaviours: function (root) {
        var self = this;
        if (this.options.clickToFront) {
            root.click(function () { self.toTop(); });
        }
        jQuery('.close', root).click(function () {
            self.hideAndUnload();
            return false;
        }).mousedown(function (evt) { evt.stopPropagation(); });
    },

    _fire: function (event) {
        this.options[event].call(this);
    }
};
//jquery提示框
function bAlert(text, func) {
    Boxy.alert(text, func, {
        modal: false,
        title: '提示',
        unloadOnHide: true
    });
}
//jquery 确认框(确定执行方法)
function bConfirm(text, func, title) {
    Boxy.confirm(text, func, {
        modal: false,
        title: title,
        unloadOnHide: true
    });
}
//jquery 确认框(确定+取消执行方法)
function bfConfirm(text, func, fund, title) {
    Boxy.fullConfirm(text, func, fund, {
        modal: false,
        title: title,
        unloadOnHide: true
    });
}
//jquery 弹出框
function showWindow(target, type, title, titletype) {
    if (type == null || type == "undifine" || type == '') { type = "url"; }
    if (title) {
        if (type == "url") { options = { modal: true, unloadOnHide: true, title: title, titletype: titletype }; Boxy.load(target, options); }
        if (type == "div") { options = { modal: true, title: title }; new Boxy($("#" + target), options) }
    } else {
        if (type == "url") { options = { modal: true, unloadOnHide: true }; Boxy.load(target, options); }
        if (type == "div") { options = { modal: true }; new Boxy($("#" + target), options) }
    }
}