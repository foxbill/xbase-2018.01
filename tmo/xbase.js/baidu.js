//2011-7-25 zhangying
(function () {
    function load_script(xyUrl, callback) {
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = xyUrl;
        //借鉴了jQuery的script跨域方法
        script.onload = script.onreadystatechange = function () {
            if ((!this.readyState || this.readyState === "loaded" || this.readyState === "complete")) {
                callback && callback(window.BMapConvertValues);
                //   window.BMapConvertValues = null;
                // Handle memory leak in IE
                script.onload = script.onreadystatechange = null;
                if (head && script.parentNode) {
                    head.removeChild(script);
                }
            }
        };
        // Use insertBefore instead of appendChild  to circumvent an IE6 bug.
        head.insertBefore(script, head.firstChild);
    }

    function stay(ms) {
        var start = (new Date()).getTime();    //结束时间
        while ((new Date()).getTime() - start < ms) {
        }
    }

    function transMore(points, type, callback) {
        var xyUrl = "http://api.map.baidu.com/ag/coord/convert?from=" + type + "&to=4&mode=1";
        var xs = [];
        var ys = [];
        var maxCnt = 20; //每次发送的最大个数

        function test(ret) {
            if (window.BMapConvertValues.length == points.length)
                callback && callback(window.BMapConvertValues);
        }

        var send = function () {
            var url = xyUrl + "&x=" + xs.join(",") + "&y=" + ys.join(",") + "&callback=callback";
            //动态创建script标签
            load_script(url, test);
            xs = [];
            ys = [];
        }



        for (var index in points) {
            if (index % maxCnt == 0 && index != 0) {
                send();
                stay(1100);
            }
            xs.push(points[index].lng);
            ys.push(points[index].lat);
            if (index == points.length - 1) {
                send();
            }
        }

    }


    window.BMap = window.BMap || {};
    BMap.Convertor = {};
    BMap.Convertor.transMore = transMore;

    window.BMapConvertValues = [];
    window.callback = function (xyResults) {
        window.BMapConvertValues = window.BMapConvertValues.concat(xyResults);
    }
})();


function baiduMap(el) {
    var _this = this;
    var gbdMap = new BMap.Map(el);

    gbdMap.centerAndZoom(new BMap.Point(105.195586, 25.440839), 12);
    gbdMap.clearOverlays();
    gbdMap.addControl(new BMap.NavigationControl());  //添加默认缩放平移控件
    gbdMap.addControl(new BMap.ScaleControl({ anchor: BMAP_ANCHOR_TOP_RIGHT }));                    // 右上
    gbdMap.setMapStyle({ style: 'grassgreen' });
    // gbdMap.addControl(new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP] }));     //2D图，卫星图

    gbdMap.addControl(new BMap.MapTypeControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT }));    //左上角，默认地图控件
    gbdMap.setCurrentCity("兴仁县");   //由于有3D图，需要设置城市哦


    function getSecDeg(sec) {
        //25°22′43.40″
        var degPos = sec.indexOf("°");
        var minPos = sec.indexOf("′");
        var secPos = sec.indexOf("″");
        var deg = Number(sec.substring(0, degPos));
        var min = Number(sec.substring(degPos + 1, minPos));
        var ss = Number(sec.substring(minPos + 1, secPos));
        return deg + (min + ss / 60) / 60;
    }

    this.drawGpsMarker = function (long, lat, title) {

        var points = [];
        var x = getSecDeg(long); //经度
        var y = getSecDeg(lat); //纬度
        var point = new BMap.Point(x, y);
        points.push(point);

        function callback(xyResults) {
            var xyResult = null;
            for (var i = 0; i < xyResults.length; i++) {
                xyResult = xyResults[i];
                if (xyResult.error != 0) { continue; } //出错就直接返回;
                var point = new BMap.Point(xyResult.x, xyResult.y);
                gbdMap.centerAndZoom(point, 16);
                var marker = new BMap.Marker(point);  // 创建标注
                gbdMap.addOverlay(marker);               // 将标注添加到地图中
                //    marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                var label = new BMap.Label(title, { offset: new BMap.Size(20, 0) });
                marker.setLabel(label);

            }
        }

        BMap.Convertor.transMore(points, 0, callback);
    }

    this.drawIconByGps = function (data, iconSrc) {

        var points = [];
        for (var i = 0; i < data.rows.length; i++) {
            var row = data.rows[i];
            var x = getSecDeg(row[data.longField]); //经度
            var y = getSecDeg(row[data.latField]); //纬度
            var point = new BMap.Point(x, y);
            points.push(point);
        }

        function callback(xyResults) {
            var xyResult = null;
            for (var i = 0; i < xyResults.length; i++) {
                var row = data.rows[i];
                xyResult = xyResults[i];
                if (xyResult.error != 0) { continue; } //出错就直接返回;
                row.x = xyResult.x;
                row.y = xyResult.y;
            }
            data.longField = "x";
            data.latField = "y";
            gBaiduMap.drawIcon(data, iconSrc);
        }

        BMap.Convertor.transMore(points, 0, callback);

    }

    this.drawIcon = function (data, iconSrc) {
        if (!iconSrc || iconSrc == undefined)
            iconSrc = "images/dg.png";

        // gbdMap.clearOverlays();
        for (var i = 0; i < data.rows.length; i++) {
            var row = data.rows[i];
            var x = row[data.longField]; //经度
            var y = row[data.latField]; //纬度
            var GanTaBianHao = row["GanTaBianHao"];

            //线杆1
            //var pt = new BMap.Point(105.3056361111, 25.37895833333);
            var pt = new BMap.Point(x, y);


            if (i == 0)
                gbdMap.centerAndZoom(pt, 16);

            //真实经纬度转成百度坐标

            var myIcon = new BMap.Icon(iconSrc, new BMap.Size(32, 32));
            var marker = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
            marker.infoWindow = new BMap.InfoWindow("<img src='images/001jj.jpg'/><p style='font-size:14px;'>" + GanTaBianHao + "</p><p style='font-size:12px;'>横担数：</p><p style='font-size:12px;'>令克数：</p><p style='font-size:12px;'>刀闸：</p>");
            marker.addEventListener("click", function () { this.openInfoWindow(this.infoWindow); });
            gbdMap.addOverlay(marker);              // 将标注添加到地图中
        }
    }

    function polyLine() {
        // 百度地图API功能
        gbdMap.centerAndZoom(new BMap.Point(105.195586, 25.440839), 12);
        gbdMap.clearOverlays();
        gbdMap.addControl(new BMap.NavigationControl());  //添加默认缩放平移控件
        gbdMap.addControl(new BMap.ScaleControl({ anchor: BMAP_ANCHOR_TOP_RIGHT }));                    // 右上
        gbdMap.setMapStyle({ style: 'grassgreen' });
        // gbdMap.addControl(new BMap.MapTypeControl({ mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP] }));     //2D图，卫星图

        gbdMap.addControl(new BMap.MapTypeControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT }));    //左上角，默认地图控件
        gbdMap.setCurrentCity("兴仁县");   //由于有3D图，需要设置城市哦
        // gbdMap.setMapStyle({ style: 'midnight' });
        //创建供所
        var pt = new BMap.Point(105.316108, 25.378926);
        var myIcon = new BMap.Icon("images/gds.png", new BMap.Size(60, 60));
        var marker3 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
        gbdMap.addOverlay(marker3);              // 将标注添加到地图中

        //（创建信息窗口）
        var infoWindow3 = new BMap.InfoWindow("<img src='images/tjgds.jpg'/><p style='font-size:14px;'>屯脚供电所</p><p style='font-size:12px;'>供电能力：</p><p style='font-size:12px;'>供电量：</p><p style='font-size:12px;'>10kv线路数：</p>");
        marker3.addEventListener("click", function () { this.openInfoWindow(infoWindow3); });


        //创建供所
        var pt = new BMap.Point(105.11601, 25.359697);
        var myIcon = new BMap.Icon("images/gds.png", new BMap.Size(60, 60));
        var marker4 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
        gbdMap.addOverlay(marker4);              // 将标注添加到地图中

        //（创建信息窗口）
        var infoWindow4 = new BMap.InfoWindow("<img src='images/tjgds.jpg'/><p style='font-size:14px;'>雨樟供电所</p><p style='font-size:12px;'>供电能力：</p><p style='font-size:12px;'>供电量：</p><p style='font-size:12px;'>10kv线路数：</p>");
        marker4.addEventListener("click", function () { this.openInfoWindow(infoWindow4); });

        //报警
        var pt = new BMap.Point(105.11601, 25.359697);
        var myIcon = new BMap.Icon("images/bj.gif", new BMap.Size(32, 32));
        var marker5 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
        gbdMap.addOverlay(marker5);              // 将标注添加到地图中

        var infoWindow5 = new BMap.InfoWindow("<p style='font-size:14px;'>故障情况</p><p style='font-size:12px;'>故障设备：</p><p style='font-size:12px;'>故障描述：</p>");
        marker5.addEventListener("click", function () { this.openInfoWindow(infoWindow5); });


        var polyline = new BMap.Polyline([
       new BMap.Point(105.3056361111, 25.37895833333),
       new BMap.Point(105.30818888889, 25.37868055555556),
       new BMap.Point(105.3088972222, 25.378036111111),
       new BMap.Point(105.31322222222, 25.3736833333333333),
       new BMap.Point(105.3178722222, 25.365663889)
       ], {
           strokeColor: "blue",
           strokeWeight: 6,
           strokeOpacity: 0.5
       });
        gbdMap.addOverlay(polyline);

        var infoWindow12 = new BMap.InfoWindow("<p style='font-size:14px;'>10kv屯脚线</p><p style='font-size:12px;'>杆塔数：</p><p style='font-size:12px;'>特殊区段：</p><p style='font-size:12px;'>本月故障：</p>");
        polyline.addEventListener("click", function (e) {
            gbdMap.openInfoWindow(infoWindow12, e.point);
        });

        var tsqd;
        function drawQd() {
            tsqd = new BMap.Polyline([
            //     new BMap.Point(105.3056361111, 25.37895833333),
            //    new BMap.Point(105.30818888889, 25.37868055555556),
          new BMap.Point(105.3088972222, 25.378036111111),
          new BMap.Point(105.31322222222, 25.3736833333333333),
            //     new BMap.Point(105.3178722222, 25.365663889)
        ], {
            strokeColor: "red",
            strokeWeight: 2,
            strokeOpacity: 1
        });
            gbdMap.addOverlay(tsqd);

        }
        drawQd();

    }
    //    var marker1 = new BMap.Marker(new BMap.Point(105.195586, 25.440839));  // 创建标注
    //    gbdMap.addOverlay(marker1);              // 将标注添加到地图中

    //    //创建信息窗口
    //    var infoWindow1 = new BMap.InfoWindow("<a href='http://www.baidu.com'>兴仁供电局<a>");
    //    marker1.addEventListener("click", function () { this.openInfoWindow(infoWindow1); });



    //创建供电局
    this.drawPointMsg = function (rec, longField, latField, imgField) {
        var x = rec[longField];
        var y = rec[latField];
        var img = rec[imgField];
        var pt = new BMap.Point(x, y);

        var myIcon = new BMap.Icon(img, new BMap.Size(32, 32));
        var marker2 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
        gbdMap.addOverlay(marker2);              // 将标注添加到地图中
        var label = new BMap.Label(rec["MingCheng"], { offset: new BMap.Size(32, -16) });
        marker2.setLabel(label);

        //（创建信息窗口）
        var html = "<img src='" + img + "'/>";
        for (var fld in rec) {
            if (fld != longField && fld != latField && fld != imgField) {
                html += "<p>" + fld + ":" + rec[fld] + "</p>";
            }
        }
        var infoWindow2 = new BMap.InfoWindow(html);
        marker2.addEventListener("click", function () { this.openInfoWindow(infoWindow2); });
        gbdMap.centerAndZoom(pt, 18);
    }

    function centerGdj() {
        gbdMap.centerAndZoom(new BMap.Point(105.195586, 25.440839), 12);
    }
    function centerTjs() {
        gbdMap.centerAndZoom(new BMap.Point(105.316108, 25.378926), 15);
    }
    function centerTj10kv() {
        gbdMap.centerAndZoom(new BMap.Point(105.31322222222, 25.3736833333333333), 16);
    }
    function centerYzs() {
        gbdMap.centerAndZoom(new BMap.Point(105.11601, 25.359697), 15);
    }

}
