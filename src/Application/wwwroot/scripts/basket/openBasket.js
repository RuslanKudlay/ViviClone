'use strict';

$(document).ready(function () {

    $('#openBasket').click(function () {

        $.ajax({
            url: ShopUrlSettings.GetModalBasketURL,
            type: 'POST',
            success: function (modal) {
                $('#modal-dialog').html(modal);
                displayBasket();
            },
            error: function (error) {
                console.error(error);
            }
        });        
    })
});

function displayBasket() {
    $.ajax({
        url: ShopUrlSettings.OpenBasketURL,
        type: 'GET',
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
}