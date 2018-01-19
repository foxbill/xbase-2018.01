/*
 *@yqdialog - Dialog Window v2.0.2 - Date : 2009-8-19
 *@Copyright yqcore.js (c) 2009 By LiHuiGang Reserved
 */
(function() { var g = window.yqcore = window.J = function(a, d) { return g.ret.init(a, d) }; g.ret = g.prototype = { init: function(a, d) { a = (a == 'body') ? document.body : (a == 'doc') ? document : a; if ('string' == typeof (a)) { if (a.indexOf('#') == 0) { var b = (d || document).getElementById(a.substr(1)); if (b) return b; else return null } var b = (d || document).getElementById(a); if (b) return g(b); else return null } else { this[0] = a; this.length = 1; return this } }, html: function(t) { if (t) { this[0].innerHTML = t; return this } else return this[0].innerHTML }, isnl: function() { var v = this[0].value; return (v == '' || v.length == 0) ? true : false }, val: function(v) { if (v) { this[0].value = v; return this } else return this[0].value }, acls: function(c, p) { this[0].className = p ? this[0].className + ' ' + c : c; return this }, rcls: function() { var a = g.ie ? 'className' : 'class'; this[0].removeAttribute(a, 0); return this }, crte: function(e) { return this[0].createElement(e) }, apch: function(c, y) { switch (y) { case 'pr': return this[0].insertBefore(c, this[0].firstChild); break; case 'be': return this[0].parentNode.insertBefore(c, this[0]); break; case 'af': return this[0].parentNode.insertBefore(c, this[0].nextSibling); break; default: return this[0].appendChild(c); break } }, stcs: function(d, s) { if (typeof (d) == 'object') { for (var n in d) this[0].style[n] = d[n]; return this } else { this[0].style[d] = s; return this } }, gtcs: function(p) { if (g.ie) return this[0].currentStyle[p]; else return this[0].ownerDocument.defaultView.getComputedStyle(this[0], '').getPropertyValue(p) }, gtag: function(n) { return this[0].getElementsByTagName(n) }, attr: function(k, v) { if (typeof (k) == 'object') { for (var n in k) this[0][n] = k[n]; return this } if (v) { this[0].setAttribute(k, v, 0); return this } else { var a = this[0].attributes[k]; if (a == null || !a.specified) return ''; return this[0].getAttribute(k, 2) } }, ratt: function(n) { var a = this[0].attributes[n]; if (a == null || !a.specified) return this; this[0].removeAttribute(n, 0); return this }, aevt: function(n, f) { if (g.ie) this[0].attachEvent('on' + n, f); else this[0].addEventListener(n, f, false); return this }, revt: function(n, f) { if (g.ie) this[0].detachEvent('on' + n, f); else this[0].removeEventListener(n, f, false); return this }, alnk: function(c) { if (g.ie) return this[0].createStyleSheet(c).owningElement; else { var e = this[0].createElement('link'); e.rel = 'stylesheet'; e.type = 'text/css'; e.href = c; this[0].getElementsByTagName('head')[0].appendChild(e); return e } } }; g.ret.init.prototype = g.ret; g.exend = g.ret.exend = function() { var a = arguments[0] || {}, i = 1, length = arguments.length, deep = false, options; if (a.constructor == Boolean) { deep = a; a = arguments[1] || {}; i = 2 } if (typeof a != 'object' && typeof a != 'function') a = {}; if (length == i) { a = this; --i } for (; i < length; i++) if ((options = arguments[i]) != null) for (var b in options) { var c = a[b], copy = options[b]; if (a === copy) continue; if (deep && copy && typeof copy == 'object' && !copy.nodeType) a[b] = g.extend(deep, c || (copy.length != null ? [] : {}), copy); else if (copy !== undefined) a[b] = copy } return a }; g.ret.exend({ stopac: function(o) { if (g.ie) { o = Math.round(o * 100); this[0].style.filter = (o > 100 ? '' : 'alpha(opacity=' + o + ')') } else this[0].style.opacity = o }, addentex: function(n, l, p) { if (g.ie) { var o = {}; o.source = this[0]; o.params = p || []; o.listen = function(a) { return l.apply(o.source, [a].concat(o.params)) }; if (g.clean) g.clean.items(null, function() { o.source = null; o.params = null }); this[0].attachEvent('on' + n, o.listen); this[0] = null; p = null } else this[0].addEventListener(n, function(e) { l.apply(this[0], [e].concat(p || [])) }, false); return this }, click: function(f) { this[0].onclick = f; return this }, blur: function(f) { this[0].onblur = f; return this }, focus: function(f) { if (f) this[0].onfocus = f; else this[0].focus(); return this }, msdown: function(f) { this[0].onmousedown = f; return this }, msmove: function(f) { this[0].onmousemove = f; return this }, msover: function(f) { this[0].onmouseover = f; return this }, msout: function(f) { this[0].onmouseout = f; return this }, msup: function(f) { this[0].onmouseup = f; return this }, submit: function(f) { if (f) this[0].onsubmit = f; else this[0].onsubmit(); return this }, cmenu: function(f) { this[0].oncontextmenu = f; return this }, hover: function(r, t) { this[0].onmouseover = r; this[0].onmouseout = t; return this } }); g.exend({ build: '1.0.0', author: 'LiHuiGang', path: function(t) { t = t || 'yqcore.js'; var a, len, sc = g('doc').gtag('script'); for (var i = 0; i < sc.length; i++) { a = sc[i].src.substr(0, g.inde(sc[i].src.toLowerCase(), t)); len = a.lastIndexOf('/'); if (len > 0) a = a.substr(0, len + 1); if (a) break } if (g.ie && g.inde(a, '../') != -1) { var b = window.location.href; b = b.substr(0, b.lastIndexOf('/')); while (g.inde(a, '../') >= 0) { a = a.substr(3); b = b.substr(0, b.lastIndexOf('/')) } return b + '/' + a } else return a }, idtd: function(d) { return ('CSS1Compat' == (d.compatMode || 'CSS1Compat')) }, rech: function(c) { if (c) return c.parentNode.removeChild(c) }, gtev: function() { if (g.ie) return window.event; var a = this.gtev.caller; while (a != null) { var b = a.arguments[0]; if (b && (b + '').indexOf('Event') >= 0) return b; a = a.caller } return null }, trim: function(t) { return (t || '').replace(/^\s+|\s+$/g, '') }, inde: function(t, s) { return t.indexOf(s) }, edoc: function(a) { return a.ownerDocument || a.document }, ewin: function(a) { return this.dwin(this.edoc(a)) }, dwin: function(d) { if (g.sa && !d.parentWindow) this.fixw(window.top); return d.parentWindow || d.defaultView }, fixw: function(w) { if (w.document) w.document.parentWindow = w; for (var i = 0; i < w.frames.length; i++) g.fixw(w.frames[i]) }, vsiz: function(a) { if (g.ie) { var b, doc = a.document.documentElement; if (doc && doc.clientWidth) b = doc; else b = a.document.body; if (b) return { w: b.clientWidth, h: b.clientHeight }; else return { w: 0, h: 0} } else return { w: a.innerWidth, h: a.innerHeight} }, spos: function(w) { if (g.ie) { var a = w.document; oPos = { x: a.documentElement.scrollLeft, y: a.documentElement.scrollTop }; if (oPos.x > 0 || oPos.y > 0) return oPos; return { x: a.body.scrollLeft, y: a.body.scrollTop} } else return { x: w.pageXOffset, y: w.pageYOffset} }, dpos: function(w, n) { var x = 0, y = 0, cn = n, pn = null, cw = g.ewin(cn); while (cn && !(cw == w && (cn == w.document.body || cn == w.document.documentElement))) { x += cn.offsetLeft - cn.scrollLeft; y += cn.offsetTop - cn.scrollTop; if (g.op) { var a = pn; while (a && a != cn) { x -= a.scrollLeft; y -= a.scrollTop; a = a.parentNode } } pn = cn; if (cn.offsetParent) cn = cn.offsetParent; else { if (cw != w) { cn = cw.frameElement; pn = null; if (cn) cw = cn.contentWindow.parent } else cn = null } } if (g(w.document.body).gtcs('position') != 'static' || (g.ie && g.gtan(n) == null)) { x += w.document.body.offsetLeft; y += w.document.body.offsetTop } return { 'x': x, 'y': y} }, gtan: function(e) { var a = e; while (a != g.edoc(a).documentElement) { if (g(a).gtcs('position') != 'static') return a; a = a.parentNode } return null }, canc: function(e) { if (g.ie) return false; else { if (e) e.preventDefault() } }, empty: function(t) { return (t == '' || t.length == 0) ? true : false }, dismn: function(e) { var a = e || window.event, el = a.srcElement || a.target, tn = el.tagName; if (!((tn == 'INPUT' && el.type == 'text') || tn == 'TEXTAREA')) { if (g.ie) return false; else { if (e) e.preventDefault() } } }, nosel: function(o) { if (g.ie) { o.unselectable = 'on'; var e, i = 0; while ((e = o.all[i++])) { switch (e.tagName.toLowerCase()) { case 'iframe': case 'textarea': case 'input': case 'select': break; default: e.unselectable = 'on' } } } else { if (g.mz) o.style.MozUserSelect = 'none'; else if (g.sa) o.style.KhtmlUserSelect = 'none'; else o.style.userSelect = 'none' } }, gtvod: function() { if (g.ie) return (g.i7 ? '' : 'javascript:\'\''); return 'javascript:void(0)' } }); var j = navigator.userAgent.toLowerCase(); g.exend({ ie: /msie/.test(j) && !/opera/.test(j), i7: (j.match(/msie (\d+)/) || [])[1] >= 7 && !/opera/.test(j), ch: /chrome/.test(j), op: /opera/.test(j), sa: /webkit/.test(j) && !/chrome/.test(j), mz: /mozilla/.test(j) && !/(compatible|webkit)/.test(j) }); g.exend({ cleanup: function() { if (window._yqcleanobj) this.citem = window._yqcleanobj.citem; else { this.citem = []; window._yqcleanobj = this; J(window).addentex('unload', this.yq_clean) } } }); g.exend(g.cleanup.prototype, { items: function(a, b) { this.citem.push([a, b]) }, yq_clean: function() { if (!this._yqcleanobj) return; var a = this._yqcleanobj.citem; while (a.length > 0) { var b = a.pop(); if (b) b[1].call(b[0]) } this._yqcleanobj = null; g = null; if (CollectGarbage) CollectGarbage() } }); if (g.ie) g.clean = new g.cleanup(); J.exend({ panel: function(b, w) { this._win = window; var a, doc, r_win = [this._win]; if (b) { while (this._win.parent && this._win.parent != this._win) { try { if (this._win.parent.document.domain != document.domain) break } catch (e) { break } this._win = this._win.parent; r_win.push(this._win) } } if (w) { for (var i = 0; i < w.length; i++) r_win.push(w[i]) } a = this._ifrm = J(this._win.document).crte('iframe'); J(a).attr({ src: 'javascript:void(0)', frameBorder: 0, scrolling: 'no' }).stcs({ display: 'none', position: 'absolute', zIndex: 19700 }); J(this._win.document.body).apch(a); doc = this._doc = a.contentWindow.document; if (J.ie) g.clean.items(this, this.p_clean); var c = ''; if (J.sa) c = '<base href="' + window.document.location + '">'; doc.open(); doc.write('<html><head>' + c + '<\/head><body style="margin:0px;padding:0px;"><\/body><\/html>'); doc.close(); for (var i = 0; i < r_win.length; i++) J(r_win[i].document).addentex('click', this.hide, this); J(doc).aevt('contextmenu', J.dismn); this._main = J(doc.body).apch(doc.createElement('div')); this._main.style.cssFloat = 'left' } }); J.exend(J.panel.prototype, { applnk: function(l) { J(this._doc).alnk(l) }, show: function(x, y, e, w, h) { var a = this._main, iw, ih; J(this._ifrm).stcs('display', 'block'); J(a).stcs({ width: w ? w + 'px' : '', height: h ? h + 'px' : '' }); iw = a.offsetWidth; ih = a.offsetHeight; if (!w) this._ifrm.style.width = '1px'; if (!h) this._ifrm.style.height = '1px'; iw = a.offsetWidth || a.firstChild.offsetWidth; var b = e.nodeType == 9 ? J.idtd(e) ? e.documentElement : e.body : e; var c = J.dpos(this._win, b); x += c.x; y += c.y; var d = J.vsiz(this._win), sp = J.spos(this._win), vh = d.h + sp.y, vw = d.w + sp.x; if ((x + iw) > vw) x -= x + iw - vw; if ((y + ih) > vh) y -= y + ih - vh; J(this._ifrm).stcs({ left: x + 'px', top: y + 'px', width: iw + 'px', height: ih + 'px' }) }, hide: function(e, a) { J(a._ifrm).stcs('display', 'none') }, p_clean: function() { this._main = null; this._doc = null; this._ifrm = null; this._win = null } }); g.ajax = g.A = { geth: function() { try { return new ActiveXObject('Msxml2.XMLHTTP') } catch (e) { } try { return new XMLHttpRequest() } catch (e) { } return null }, send: function(u, m, p, f, x) { m = m ? m.toLocaleUpperCase() : 'GET'; x = x ? x : 0; p = p ? p + '&uuid=' + new Date().getTime() : null; var a = (typeof (f) == 'function'), ret; var b = this.geth(); b.open(m, u, a); if (a) { b.onreadystatechange = function() { if (b.readyState == 4) { ret = (x == 0) ? b.responseText : b.responseXML; f(ret); delete (b); return } else return false } } if (m == 'GET') b.send(null); else { b.setRequestHeader('content-type', 'application/x-www-form-urlencoded'); if (p) b.send(p); else return false } if (!a) { if (b.readyState == 4 && b.status == 200) { ret = (x == 0) ? b.responseText : b.responseXML; delete (b); return ret } else return false } } } })();
J.exend({

    dialog: (function() {
        var twin = window.parent, cover;
        while (twin.parent && twin.parent != twin) {
            try { if (twin.parent.document.domain != document.domain) break; }
            catch (e) { break; }
            twin = twin.parent;
        }
        var tdoc = twin.document;

        var restyle = function(el) {
            el.style.cssText = 'margin:0;padding:0;background-image:none;background-color:transparent;border:0;';
        };

        var getzi = function() {
            if (!J.dialog.cfg.bzi) J.dialog.cfg.bzi = 1999; return ++J.dialog.cfg.bzi;
        };

        var resizehdl = function() {
            if (!cover) return; var rel = J.idtd(tdoc) ? tdoc.documentElement : tdoc.body;
            J(cover).stcs({
                width: Math.max(rel.scrollWidth, rel.clientWidth, tdoc.scrollWidth || 0) - 1 + 'px',
                height: Math.max(rel.scrollHeight, rel.clientHeight, tdoc.scrollHeight || 0) - 1 + 'px'
            });
        };

        return {
            cfg: { bzi: null, opac: 0.50, bgcolor: '#fff' }, indoc: {}, infrm: {}, inwin: {},
            get: function(d) {
                if (!d || 'object' != typeof d || !d.id || J('#yq_' + d.id, tdoc)) return;
                if (d.cover) this.dcover(); else { if (cover) cover = null; }
                d.width = d.width || 400; d.height = d.height || 300; d.title = d.title || 'yqdialog弹出窗口';

                var dinfo = { tit: d.title, page: d.page, link: d.link, html: d.html, win: window, top: twin, msg: d.msg }
                var cize = J.vsiz(twin), pos = J.spos(twin);

                var itop = d.top ? pos.y + d.top : Math.max(pos.y + (cize.h - d.height - 20) / 2, 0);
                var ileft = d.left ? pos.x + d.left : Math.max(pos.x + (cize.w - d.width - 20) / 2, 0);

                var dfrm = J(tdoc).crte('iframe'); restyle(dfrm);
                J(dfrm).attr({ id: 'yq_' + d.id, frameBorder: 0 }).stcs({
                    top: itop + 'px', left: ileft + 'px', position: 'absolute',
                    width: d.width + 'px', height: d.height + 'px', zIndex: getzi()
                });
                dfrm._dlgargs = dinfo; J(tdoc.body).apch(dfrm); var doc = dfrm.contentWindow.document;

                doc.open();
                var str = "";
                if (dinfo.page == null && dinfo.msg != null) {//文本形式
                    doc.writeln([
			    '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">',
				'<html xmlns="http://www.w3.org/1999/xhtml">',
				'<head>',
				    '<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>',
					'<link type="text/css" href="' + J.path() + 'yqdialog/yqdialog.css" rel="stylesheet"/>',
					'<script type="text/javascript">',
'var W=frameElement._dlgargs.win,dlgcover=W.J.dialog.gcover(),l=window.document;function A(){return frameElement._dlgargs};window.focus();if(W.J.ie){try{document.execCommand("BackgroundImageCache",false,true)}catch(e){}};var recontze=function(){if(W.J.ie&&!W.J.i7){W.J("contain",l).stcs({width:document.body.offsetWidth-2+"px",height:document.body.offsetHeight-2+"px"})}var h=W.J("#contain",l).offsetHeight;h-=W.J("#dtit",l).offsetHeight;h-=W.J("#dfoot",l).offsetHeight;W.J("dinner",l).stcs({height:Math.max(h-9,0)+"px"})};var crtel=function(t,l,w,h){var o=W.J(A().top.document).crte("div");W.J(o).stcs({position:"absolute",top:t+"px",left:l+"px",border:"1px solid #000",width:w+"px",height:h+"px",zIndex:W.J.dialog.cfg.bzi+1,backgroundColor:"#999"}).stopac(0.30);W.J(A().top.document.body).apch(o);return o};var drag=function(){var e=[],lacoor,curpos,tdark;var f=function(){for(var i=0;i<e.length;i++){W.J(e[i].document).revt("mousemove",g);W.J(e[i].document).revt("mouseup",h)}};var g=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;var b={x:a.screenX,y:a.screenY};curpos={x:curpos.x+(b.x-lacoor.x),y:curpos.y+(b.y-lacoor.y)};lacoor=b;W.J(tdark).stcs({left:curpos.x+"px",top:curpos.y+"px"})};var h=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;f();W.J.rech(tdark);lacoor=null;tdark=null;W.J(frameElement).stcs({left:curpos.x+"px",top:curpos.y+"px"})};return{downhdl:function(a){var b=null;if(!a){b=W.J.edoc(this).parentWindow;a=b.event}else b=a.view;var c=a.srcElement||a.target;if(c.id=="xbtn")return;var d=frameElement.offsetWidth,fh=frameElement.offsetHeight;curpos={x:frameElement.offsetLeft,y:frameElement.offsetTop};lacoor={x:a.screenX,y:a.screenY};tdark=crtel(curpos.y,curpos.x,d,fh);for(var i=0;i<e.length;i++){W.J(e[i].document).aevt("mousemove",g);W.J(e[i].document).aevt("mouseup",h)}if(a.preventDefault)a.preventDefault();else a.returnValue=false},reghdl:function(w){e.push(w)}}}();var resize=function(){var d=[],lacoor,curpos,tdark,frsize;var e=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;var b={x:a.screenX,y:a.screenY};frsize={w:b.x-lacoor.x,h:b.y-lacoor.y};if(frsize.w<200||frsize.h<100){frsize.w=200;frsize.h=100};W.J(tdark).stcs({width:frsize.w+"px",height:frsize.h+"px",top:curpos.y+"px",left:curpos.x+"px"})};var f=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;for(var i=0;i<d.length;i++){W.J(d[i].document).revt("mousemove",e);W.J(d[i].document).revt("mouseup",f)}W.J(frameElement).stcs({width:frsize.w+"px",height:frsize.h+"px"});recontze();W.J.rech(tdark);lacoor=null;tdark=null;if(W.J.ie&&!W.J.i7&&W.J("#frmain",l))W.J("#frmain",l).height=W.J("#dinner",l).style.height};return{downhdl:function(a){var b=null;if(!a){b=W.J.edoc(this).parentWindow;a=b.event}else b=a.view;var c=frameElement.offsetWidth,fh=frameElement.offsetHeight;curpos={x:frameElement.offsetLeft,y:frameElement.offsetTop};frsize={w:c,h:fh};lacoor={x:a.screenX-c,y:a.screenY-fh};tdark=crtel(curpos.y,curpos.x,c,fh);for(var i=0;i<d.length;i++){W.J(d[i].document).aevt("mousemove",e);W.J(d[i].document).aevt("mouseup",f)}if(a.preventDefault)a.preventDefault();else a.returnValue=false},reghdl:function(w){d.push(w)}}}();(function(){var d=function(a){a.onkeydown=function(e){e=e||event||this.parentWindow.event;switch(e.keyCode){case 27:cancel();return false;case 9:W.J.canc(e);return false}return true}};window.onload=function(){W.J("throbber",l).stcs("visibility","");loadinnfrm();W.J(document).cmenu(W.J.canc);if(W.J.ie)W.J(window.document).msdown(g);else W.J(window).msdown(g);W.J("dtit",l).msdown(drag.downhdl);drag.reghdl(window);drag.reghdl(A().top);drag.reghdl(W);W.J("dark",l).msdown(resize.downhdl);resize.reghdl(window);resize.reghdl(A().top);resize.reghdl(W);if(A().link||A().html)W.J("throbber",l).stcs("visibility","hidden");h();recontze();d(document);var a=frameElement.id.substr(4),o=W.J.dialog;o.indoc[a]=document;o.infrm[a]=W.J("#frmain",l);o.inwin[a]=window};window.loadinnfrm=function(){if(A().html)W.J("dinner",l).html(A().html);',
'else{var a=A().link?A().link:A().page,_css=A().link?"":"style=\'visibility:hidden;\' ";W.J("dinner",l).html(A().msg)}};window.loadinndlg=function(){if(!frameElement.parentNode)return null;var a=W.J("#frmain",l),innwin=a.contentWindow,inndoc=innwin.document;W.J("throbber",l).stcs("visibility","hidden");a.style.visibility="";if(W.J.ie)W.J(inndoc).msdown(g);else W.J(innwin).msdown(g);drag.reghdl(innwin);resize.reghdl(innwin);innwin.focus();d(inndoc);return W};window.cancel=function(){return closedlg()};window.closedlg=function(){if(W.J("#frmain",l))W.J("#frmain",l).src=W.J.gtvod();W.J("throbber",l).stcs("visibility","hidden");W.J.dialog.close(window,dlgcover)};window.reload=function(a,c,b){a=a?a:W;W.J.dialog.close(window,dlgcover);if(!c)a.location.reload();else{if(!b)a.location.href=c;else a.src=c}};var g=function(a){if(!a)a=event||this.parentWindow.event;W.J(frameElement).stcs("zIndex",parseInt(W.J.dialog.cfg.bzi,10)+1);W.J.dialog.cfg.bzi=frameElement.style.zIndex;a.stopPropagation?a.stopPropagation():(a.cancelBubble=true)};var h=function(){if(W.J.ie){var a=new Image();a.src="images/btn_bg.gif"};W.J("xbtn",l).msover(function(){W.J(this).stcs("backgroundPosition","0 0")}).msout(function(){W.J(this).stcs("backgroundPosition","-22px 0")}).click(cancel);W.J("txt",l).html(A().tit);crebtn("cbtn","取 消",cancel)};window.crebtn=function(i,t,f){if(W.J("#"+i,l)){W.J(i,l).html("<span>"+t+"</span>");W.J(i,l).click(f)}else{var a=W.J(l).crte("li"),span=W.J(l).crte("span");W.J(span).html(t);W.J(a).attr("id",i).apch(span);W.J(a).msover(function(){W.J(this).stcs("backgroundPosition","0 -42px")}).msout(function(){W.J(this).stcs("backgroundPosition","0 -21px")}).click(f);W.J("btns",l).apch(a);a=span=null}};window.rembtn=function(a){if(W.J("#"+a,l))W.J.rech(W.J("#"+a,l))}})();',
					'</script>',
				'</head>',
				'<body>',
				    '<div id="contain" class="contain">',
					    '<div id="dtit" class="dlgtit"><span id="txt"></span><div id="xbtn"></div></div>',
						'<div id="dinner" class="dlginner"></div>',
						'<div id="dfoot" class="dlgfoot"><ul id="btns"><li id="dark"></li></ul></div>',
					'</div>',
					'<div id="throbber" style="position:absolute;visibility:hidden;"></div>',
				'</body>',
				'</html>'
			].join(''));
                }
                else {//页面形式
                    doc.writeln([
			    '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">',
				'<html xmlns="http://www.w3.org/1999/xhtml">',
				'<head>',
				    '<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>',
					'<link type="text/css" href="' + J.path() + 'yqdialog/yqdialog.css" rel="stylesheet"/>',
					'<script type="text/javascript">',
'var W=frameElement._dlgargs.win,dlgcover=W.J.dialog.gcover(),l=window.document;function A(){return frameElement._dlgargs};window.focus();if(W.J.ie){try{document.execCommand("BackgroundImageCache",false,true)}catch(e){}};var recontze=function(){if(W.J.ie&&!W.J.i7){W.J("contain",l).stcs({width:document.body.offsetWidth-2+"px",height:document.body.offsetHeight-2+"px"})}var h=W.J("#contain",l).offsetHeight;h-=W.J("#dtit",l).offsetHeight;h-=W.J("#dfoot",l).offsetHeight;W.J("dinner",l).stcs({height:Math.max(h-9,0)+"px"})};var crtel=function(t,l,w,h){var o=W.J(A().top.document).crte("div");W.J(o).stcs({position:"absolute",top:t+"px",left:l+"px",border:"1px solid #000",width:w+"px",height:h+"px",zIndex:W.J.dialog.cfg.bzi+1,backgroundColor:"#999"}).stopac(0.30);W.J(A().top.document.body).apch(o);return o};var drag=function(){var e=[],lacoor,curpos,tdark;var f=function(){for(var i=0;i<e.length;i++){W.J(e[i].document).revt("mousemove",g);W.J(e[i].document).revt("mouseup",h)}};var g=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;var b={x:a.screenX,y:a.screenY};curpos={x:curpos.x+(b.x-lacoor.x),y:curpos.y+(b.y-lacoor.y)};lacoor=b;W.J(tdark).stcs({left:curpos.x+"px",top:curpos.y+"px"})};var h=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;f();W.J.rech(tdark);lacoor=null;tdark=null;W.J(frameElement).stcs({left:curpos.x+"px",top:curpos.y+"px"})};return{downhdl:function(a){var b=null;if(!a){b=W.J.edoc(this).parentWindow;a=b.event}else b=a.view;var c=a.srcElement||a.target;if(c.id=="xbtn")return;var d=frameElement.offsetWidth,fh=frameElement.offsetHeight;curpos={x:frameElement.offsetLeft,y:frameElement.offsetTop};lacoor={x:a.screenX,y:a.screenY};tdark=crtel(curpos.y,curpos.x,d,fh);for(var i=0;i<e.length;i++){W.J(e[i].document).aevt("mousemove",g);W.J(e[i].document).aevt("mouseup",h)}if(a.preventDefault)a.preventDefault();else a.returnValue=false},reghdl:function(w){e.push(w)}}}();var resize=function(){var d=[],lacoor,curpos,tdark,frsize;var e=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;var b={x:a.screenX,y:a.screenY};frsize={w:b.x-lacoor.x,h:b.y-lacoor.y};if(frsize.w<200||frsize.h<100){frsize.w=200;frsize.h=100};W.J(tdark).stcs({width:frsize.w+"px",height:frsize.h+"px",top:curpos.y+"px",left:curpos.x+"px"})};var f=function(a){if(!lacoor)return;if(!a)a=W.J.edoc(this).parentWindow.event;for(var i=0;i<d.length;i++){W.J(d[i].document).revt("mousemove",e);W.J(d[i].document).revt("mouseup",f)}W.J(frameElement).stcs({width:frsize.w+"px",height:frsize.h+"px"});recontze();W.J.rech(tdark);lacoor=null;tdark=null;if(W.J.ie&&!W.J.i7&&W.J("#frmain",l))W.J("#frmain",l).height=W.J("#dinner",l).style.height};return{downhdl:function(a){var b=null;if(!a){b=W.J.edoc(this).parentWindow;a=b.event}else b=a.view;var c=frameElement.offsetWidth,fh=frameElement.offsetHeight;curpos={x:frameElement.offsetLeft,y:frameElement.offsetTop};frsize={w:c,h:fh};lacoor={x:a.screenX-c,y:a.screenY-fh};tdark=crtel(curpos.y,curpos.x,c,fh);for(var i=0;i<d.length;i++){W.J(d[i].document).aevt("mousemove",e);W.J(d[i].document).aevt("mouseup",f)}if(a.preventDefault)a.preventDefault();else a.returnValue=false},reghdl:function(w){d.push(w)}}}();(function(){var d=function(a){a.onkeydown=function(e){e=e||event||this.parentWindow.event;switch(e.keyCode){case 27:cancel();return false;case 9:W.J.canc(e);return false}return true}};window.onload=function(){W.J("throbber",l).stcs("visibility","");loadinnfrm();W.J(document).cmenu(W.J.canc);if(W.J.ie)W.J(window.document).msdown(g);else W.J(window).msdown(g);W.J("dtit",l).msdown(drag.downhdl);drag.reghdl(window);drag.reghdl(A().top);drag.reghdl(W);W.J("dark",l).msdown(resize.downhdl);resize.reghdl(window);resize.reghdl(A().top);resize.reghdl(W);if(A().link||A().html)W.J("throbber",l).stcs("visibility","hidden");h();recontze();d(document);var a=frameElement.id.substr(4),o=W.J.dialog;o.indoc[a]=document;o.infrm[a]=W.J("#frmain",l);o.inwin[a]=window};window.loadinnfrm=function(){if(A().html)W.J("dinner",l).html(A().html);',
'else{var a=A().link?A().link:A().page,_css=A().link?"":"style=\'visibility:hidden;\' ";W.J("dinner",l).html(',
'"<iframe id=\'frmain\' src=\'"+a+"\' name=\'frmain\' frameborder=\'0\' "+"width=\'100%\' height=\'100%\' scrolling=\'auto\' "+_css+"allowtransparency=\'true\'><\/iframe>"',
')}};window.loadinndlg=function(){if(!frameElement.parentNode)return null;var a=W.J("#frmain",l),innwin=a.contentWindow,inndoc=innwin.document;W.J("throbber",l).stcs("visibility","hidden");a.style.visibility="";if(W.J.ie)W.J(inndoc).msdown(g);else W.J(innwin).msdown(g);drag.reghdl(innwin);resize.reghdl(innwin);innwin.focus();d(inndoc);return W};window.cancel=function(){return closedlg()};window.closedlg=function(){if(W.J("#frmain",l))W.J("#frmain",l).src=W.J.gtvod();W.J("throbber",l).stcs("visibility","hidden");W.J.dialog.close(window,dlgcover)};window.reload=function(a,c,b){a=a?a:W;W.J.dialog.close(window,dlgcover);if(!c)a.location.reload();else{if(!b)a.location.href=c;else a.src=c}};var g=function(a){if(!a)a=event||this.parentWindow.event;W.J(frameElement).stcs("zIndex",parseInt(W.J.dialog.cfg.bzi,10)+1);W.J.dialog.cfg.bzi=frameElement.style.zIndex;a.stopPropagation?a.stopPropagation():(a.cancelBubble=true)};var h=function(){if(W.J.ie){var a=new Image();a.src="images/btn_bg.gif"};W.J("xbtn",l).msover(function(){W.J(this).stcs("backgroundPosition","0 0")}).msout(function(){W.J(this).stcs("backgroundPosition","-22px 0")}).click(cancel);W.J("txt",l).html(A().tit);crebtn("cbtn","取 消",cancel)};window.crebtn=function(i,t,f){if(W.J("#"+i,l)){W.J(i,l).html("<span>"+t+"</span>");W.J(i,l).click(f)}else{var a=W.J(l).crte("li"),span=W.J(l).crte("span");W.J(span).html(t);W.J(a).attr("id",i).apch(span);W.J(a).msover(function(){W.J(this).stcs("backgroundPosition","0 -42px")}).msout(function(){W.J(this).stcs("backgroundPosition","0 -21px")}).click(f);W.J("btns",l).apch(a);a=span=null}};window.rembtn=function(a){if(W.J("#"+a,l))W.J.rech(W.J("#"+a,l))}})();',
					'</script>',
				'</head>',
				'<body>',
				    '<div id="contain" class="contain">',
					    '<div id="dtit" class="dlgtit"><span id="txt"></span><div id="xbtn"></div></div>',
						'<div id="dinner" class="dlginner"></div>',
						'<div id="dfoot" class="dlgfoot"><ul id="btns"><li id="dark"></li></ul></div>',
					'</div>',
					'<div id="throbber" style="position:absolute;visibility:hidden;">正在加载窗口内容，请稍等....</div>',
				'</body>',
				'</html>'
			].join(''));
                }               

                doc.close();
            },

            close: function(d, c) {
                var dlg = ('object' == typeof (d)) ? d.frameElement : J('#yq_' + d);
                if (dlg) J.rech(dlg); if (c) this.hcover(c);
            },

            dcover: function() {
                cover = J(tdoc).crte('div'); restyle(cover);
                J(cover).stcs({
                    position: 'absolute', zIndex: getzi(), top: '0px',
                    left: '0px', backgroundColor: this.cfg.bgcolor
                }).stopac(this.cfg.opac);

                if (J.ie && !J.i7) {
                    var ifrm = J(tdoc).crte('iframe'); restyle(ifrm);
                    J(ifrm).attr({
                        hideFocus: true, frameBorder: 0, src: J.gtvod()
                    }).stcs({
                        width: '100%', height: '100%', position: 'absolute', left: '0px',
                        top: '0px', filter: 'progid:DXImageTransform.Microsoft.Alpha(opacity=0)'
                    });
                    J(cover).apch(ifrm);
                }

                J(twin).aevt('resize', resizehdl); resizehdl(); J(tdoc.body).apch(cover);
            },

            gcover: function() { return cover; },
            hcover: function(o) { J.rech(o); cover = null; o = null; }
        };
    })()

});



function dlgopen() {
    J.dialog.get({ id: 'test10', title: '测试窗口', msg: '错误', cover: true });
}
function dlgopen1() {
    J.dialog.get({ id: 'test1', title: '测试窗口', page: '_content/content.html' });
}
function ShowDialog(Id, Title, Url) {
    J.dialog.get({ id: Id, title: Title, width: 600, height: 500, link: Url, cover: true });
}
function ShowDialogOld(Id, Title, Url) {
    J.dialog.get({ id: Id, title: Title, page: Url, cover: true });
}

function dlgopen3() {
    J.dialog.get({ id: 'test3', title: '测试窗口', width: 500, link: 'http://www.google.com' });
}
function dlgopen4() {
    J.dialog.get({ id: 'test4', title: '测试窗口', html: '<p align="center" style="font-size:13px">yqcore框架之yqdialog弹出窗口组件。</p>' });
}
function dlgopen5() {
    J.dialog.get({ id: 'test5', title: '测试窗口', page: '_content/content.html', top: 100, left: 100 });
}
function dlgopen6() {
    J.dialog.get({ id: 'test6', title: '测试窗口', page: '_content/content1.html' });
}
function dlgopen7() {
    J.dialog.get({ id: 'test7', title: '测试窗口', page: '_content/content.html' });
}
function closdlg() {
    J.dialog.close('test7'); //为打开窗口的id
}
function dlgopen8() {
    J.dialog.get({ id: 'test8', title: '测试窗口', page: '_content/content3.html' });
}
function todlgval() {
    // J.dialog.infrm['test8'].contentWindow.document 为弹出的id为test8的窗口的内容页（也就是content3.html）的document对象。
    J('ctxt1', J.dialog.infrm['test8'].contentWindow.document).val(J('#txt1').value);
}
function dlgtoval() {
    // J.dialog.infrm['test8'].contentWindow.document 为弹出的id为test8的窗口的内容页（也就是content3.html）的document对象。
    J('#txt1').value = J('ctxt1', J.dialog.infrm['test8'].contentWindow.document).val();
}
function dlgopen9() {
    J.dialog.get({ id: 'test9', title: '测试窗口', page: '_content/content4.html' });
}
function dlgopen10() {
    J.dialog.get({ id: 'test10', title: '测试窗口', page: '_content/content5.html' });
}
function dlgopen11() {
    J.dialog.get({ id: 'test11', title: '测试窗口', page: '_content/content7.html' });
}
function tochild() {
    //child2为子窗口id，txt2为子窗口中文本框id
    J('txt2', J.dialog.infrm['child2'].contentWindow.document).val(J('#txt4').value);
}
function dlgopen12() {
    J.dialog.get({ id: 'test12', title: '测试窗口', page: '_content/content9.html' });
}
