/*!
 * Copyright (c) 2014 Ben Olson (https://github.com/bseth99/)
 * A Simple Rating Bar Widget
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 */

(function ($) {

    var RatingBar = function (element, options) {

        var $me = $(element).addClass('star-ctr');

        var $bg, $fg, steps, wd, cc,
            sw, fw, cs, cw, ini;

        $bg = $me.children('ul');
        $fg = $bg.clone().addClass('star-fg').css('width', 0).appendTo($me);
        $bg.addClass('star-bg');

        function initialize() {

            ini = true;

            // How many rating elements
            cc = $bg.children().length;

            steps = Math.floor(+($me.attr('data-steps') || 0));

            // Total width of the bar
            wd = $bg.width();

        }
    }

    $.fn.ratingbar = function (option) {

        return this.each(function () {

            var $this = $(this)
            var data = $this.data('osb.ratingbar')
            var options = typeof option == 'object' && option

            if (!data) $this.data('osb.ratingbar', (data = new RatingBar(this, options)))

        })
    }

    $.fn.ratingbar.Constructor = RatingBar

})(jQuery);
