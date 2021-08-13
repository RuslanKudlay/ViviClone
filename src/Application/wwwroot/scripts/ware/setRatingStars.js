'use strict';

$(document).ready(function () {
    
    $('.wareStarRatingContainer').html(function () {
        
        var starQuantity = 5;
        var averageRate = $(this).data('loading-text');
        if ($.type(averageRate) === "string"){
            averageRate = +averageRate.replace(",", ".");
        }        

        var stars = '';
        for (var j = 0; j < starQuantity; j++) {
            if (averageRate < (j + 0.25)) {
                var stars = stars +
                    `<span class="rate-star">
                        <i class="fa fa-star-o"></i>
                    </span>`;
            }
            else if (averageRate >= (j + 0.25) && averageRate < (j + 0.75)) {
                stars = stars +
                    `<span class="rate-star">
                        <i class="fa fa-star-half-o"></i>
                    </span>`;
            }
            else {
                stars = stars +
                    `<span class="rate-star">
                        <i class="fa fa-star"></i>
                    </span>`;
            }
        }
        return stars;
    })
});