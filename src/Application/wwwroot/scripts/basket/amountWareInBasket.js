'use strict';

$(document).ready(function () {

    function updateBasket(wareId, quantity) {
        $.ajax({
            url: ShopUrlSettings.SetCountWareInBasket,
            type: 'POST',
            data: {
                Id: wareId,
                wareCount: quantity
            },
            success: function (basket) {

                if (basket.basketItems.length <= 0) {
                    $('#modalHeader').text('Корзина пуста');
                }
                else {
                    $('#modalHeader').text('Корзина');
                }
                //To transfer value in pop-up basket
                $("#totalPrice").text(basket.totalPrice.toFixed(2) + " UAH");
            },
            error: function (error) {
                console.error(error);
            }
        });
    }

    $('body').on('click', '.decreateWare', function (e) {
        let inputQuantity = $(this).next();
        let wareId = +inputQuantity.attr("wareid");
        let currentCount = +inputQuantity.val();

        if (currentCount > 1) {
            inputQuantity.val(--currentCount);
            updateBasket(wareId, currentCount);
        }
        else {
            removeItemFromBasket(e, $(this));
        }
    });

    $('body').on('click', '.incrementWare', function () {
        let inputQuantity = $(this).prev();
        let wareId = +inputQuantity.attr("wareid");
        let currentCount = +inputQuantity.val();

        inputQuantity.val(++currentCount);
        updateBasket(wareId, currentCount);
    });
});