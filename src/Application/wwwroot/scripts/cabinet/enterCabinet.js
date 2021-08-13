'use strict';

$(document).ready(function () {

    $('.cabinetEnter').click(function () {
        $.ajax({
            url: ShopUrlSettings.GetModalEnterToCabinetURL,
            type: 'POST',
            success: function (modal) {
                $('#modalPopUp')
                    .html(modal)
                    .modal("show");
            },
            error: function (error) {
                console.error(error);
            }
        });
    })
  
});


