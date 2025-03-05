Morris.Donut.prototype.resizeHandler = function () {
    this.timeoutId = null;
    if (this.el && this.el.width() > 0 && this.el.height() > 0) {
        this.raphael.setSize(this.el.width(), this.el.height());
        return this.redraw();
    }
    else return null;
};
Morris.Donut.prototype.setData = function (data) {
    var row;
    this.data = data;
    this.values = (function () {
        var _i, _len, _ref, _results;
        _ref = this.data;
        _results = [];
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            row = _ref[_i];
            _results.push(parseFloat(row.value));
        }
        return _results;
    }).call(this);
    if (this.el && this.el.width() > 0 && this.el.height() > 0) {
        return this.redraw();
    }
    else return null;
};

$(function () {
    //Morris.Donut({
    //    element: 'morris-donut-chart',
    //    data: [{
    //        label: "Download Sales",
    //        value: 12
    //    }, {
    //        label: "In-Store Sales",
    //        value: 30
    //    }, {
    //        label: "Mail-Order Sales",
    //        value: 20
    //    }],
    //    resize: true
    //});

    //Morris.Bar({
    //    element: 'morris-bar-chart',
    //    data: [{
    //        y: '2006',
    //        a: 100,
    //        b: 90
    //    }, {
    //        y: '2007',
    //        a: 75,
    //        b: 65
    //    }, {
    //        y: '2008',
    //        a: 50,
    //        b: 40
    //    }, {
    //        y: '2009',
    //        a: 75,
    //        b: 65
    //    }, {
    //        y: '2010',
    //        a: 50,
    //        b: 40
    //    }, {
    //        y: '2011',
    //        a: 75,
    //        b: 65
    //    }, {
    //        y: '2012',
    //        a: 100,
    //        b: 90
    //    }],
    //    xkey: 'y',
    //    ykeys: ['a', 'b'],
    //    labels: ['Series A', 'Series B'],
    //    hideHover: 'auto',
    //    resize: true
    //});
    $(window).resize();
});
