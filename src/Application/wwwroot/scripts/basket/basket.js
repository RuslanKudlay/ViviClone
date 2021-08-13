'use strict';

$(document).ready(function () {
    $('.add-to-basket').click(function () {
        var wareSubUrl = $(this).attr('data-subUrl');
        $.ajax({
            url: ShopUrlSettings.AddWareToBasketURL,
            type: 'POST',
            data: {
                SubUrl: wareSubUrl
            },
            success: function (result) {
                $('#modalBody').html(result);
                $(".modal-footer").css({ "border": "0" });
                updateTopPartOfBasket();
                $("#BoostrapModal").modal("show");
                $("#BoostrapModal").addClass("fixWindowModal");
                $("body").addClass("fixBodyModal");
                $("#modalHeader").text("Корзина");
                $(".modal-footer").hide();
                $(".modal-content").css({ "width": "1000px" });
            },
            error: function (error) {
                console.error(error);
            }
        });
    });

    $('body').on('click', '.close-basket-modal', function () {
        $("#BoostrapModal").modal("hide");
        $("#BoostrapModal").removeClass("fixWindowModal");
        $("body").removeClass("fixBodyModal");
        $(".modal-footer").show();
    });

    $('body').on('click', '.clear-basket', function () {
        $.ajax({
            url: ShopUrlSettings.ClearBasketUrl,
            type: 'POST',
            success: function (basket) {
                updateTopPartOfBasket();
            },
            error: function (error) {
                console.error(error);
            }
        });
    });


    $('body').on('click', '.cart-menu', function () {
        showBasket(true);
    });

    $('body').on('click', '.delete-product-hover-cart', function () {

        var wareId = $(this).data('id');

        $.ajax({
            url: ShopUrlSettings.RemoveItemFromBasketURL,
            type: 'POST',
            data: {
                Id: wareId
            },
            success: function (basket) {
                updateTopPartOfBasket();
            },
            error: function (error) {
                console.error(error);
            }
        });
    })

    $('body').on('click', '.delete-product-cart', removeItemFromBasket)

    $('body').on('click', '.continueToOrder', function () {
        $("#modalFormOrder").modal("show");
        $("#BoostrapModal").modal("hide");
        $("body").addClass("fixBodyModal");
    })
});

function showBasket(isFullDisplay = false) {
    $.ajax({
        url: ShopUrlSettings.GetBasketURL,
        type: 'POST',
        data: {
            isFullDisplay: isFullDisplay
        },
        success: function (result) {
            $("#modalHeader").html("<h3>Корзина</h3>")
            $("#modalBody").html(result);
            $(".modal-footer").css({ "border": "0" });
            $("#BoostrapModal").modal("show");
            $("#BoostrapModal").addClass("fixWindowModal");
            $("body").addClass("fixBodyModal");
            $(".modal-footer").hide();
        },
        error: function (error) {
            console.error(error);
        }
    });
}

function removeItemFromBasket(e, that = null) {

    if (that == null) {
        that = $(this);
    }

    let wareId = that.data('id');

    $.ajax({
        url: ShopUrlSettings.RemoveItemFromBasketURL,
        type: 'POST',
        data: {
            Id: wareId
        },
        success: function (basket) {
            that.parent().parent().parent().parent().parent().remove();
            //To transfer value in pop-up basket
            $("#totalPrice").text(basket.totalPrice.toFixed(2) + " UAH");
            updateTopPartOfBasket();
            if (basket.basketItems.length <= 0) {
                $('#modalBody').html("<div style=' text-align: center;font-size:18px;'>Корзина пуста</div>");
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
}

$('#BoostrapModalButton').click(function () {
    $("#BoostrapModal").removeClass("fixWindowModal");
    $("body").removeClass("fixBodyModal");
    $(".modal-footer").show();
});

$('#closeTemporatyRegisterModalWindow').click(function () {
    $("#BoostrapModal").removeClass("fixWindowModal");
    $("body").removeClass("fixBodyModal");
});

function updateTopPartOfBasket(isFullDisplay = false) {
    $.ajax({
        url: ShopUrlSettings.GetBasketURL,
        type: 'POST',
        data: {
            isFullDisplay: isFullDisplay
        },
        success: function (result) {
            $(".header-bottom-right .cart-menu-relative").remove();
            $(".header-bottom-right").append(result);
        },
        error: function (error) {
            console.error(error);
        }
    });
  
}

function UpdateBasket() {
    let basketItems;
    $(".div-table-cart .cart_item").each(function () {
        let item = {
            "key": {
                Id: $(this).find(".quantity buttons_added input").attr("id")
            },
            "value": $(this).find(".quantity buttons_added input").val()
        };

        basketItems.push(item);       
    })

    if (!basketItems)
        return;

    $.ajax({
        url: ShopUrlSettings.UpdateBasketUrl,
        type: 'POST',
        data: basketItems,
        async:false,
        success: function (result) {           
        },
        error: function (error) {
            console.error(error);
        }
    });

}

