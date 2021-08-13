'use strict';

$(document).ready(function () {

    $('body').on('click', '.removeItemFromBasket', function () {

        var wareId = $(this).data('id');

        $.ajax({
            url: ShopUrlSettings.RemoveItemFromBasketURL,
            type: 'POST',
            data: {
                Id: wareId
            },
            success: function (basket) {

                if (basket.basketItems.length <= 0) {
                    $('#modalHeader').text('Корзина пуста');
                }
                else {
                    $('#modalHeader').text('Корзина');
                }

                showBasket(basket);
            },
            error: function (error) {
                console.error(error);
            }
        });
    })
});