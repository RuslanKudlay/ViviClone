'use strict';

$(document).ready(function () {
    //UpdateBasket();
    $('body').on('click', '.createOrder', function (e) {
        $("#BoostrapModal").modal('hide');
        $(".cart-amount").text("0");
        $(".list-hover-cart > li").remove();
        $(".subtotal-hover-cart").remove();
        $(".button-cart-hover").remove();
        $(".list-hover-cart").append("<span>Корзина пуста</span>");
        e.preventDefault();

        let firstName = $('#firstNameNewCutomer').val();
        let secondName = $('#secondNameNewCutomer').val();
        let phone = $('#phoneNewCutomer').val();
        let email = $('#emailNewCutomer').val();

        if (firstName == "" || secondName == "" || phone == "" || email == "") {
            $.ajax({
                url: ShopUrlSettings.NewOrder,
                type: 'POST',
                data: {},
                dataType: 'json',
                success: function (response) {
                    DisplayOrderCreateResult(response, true, true);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
        else {
            $.ajax({
                url: ShopUrlSettings.NewOrder,
                type: 'POST',
                data: {
                    FirstName: firstName,
                    SecondName: secondName,
                    Phone: phone,
                    Email: email
                },
                dataType: 'json',
                success: function (response) {
                    DisplayOrderCreateResult(response, true, true);
                },
                error: function (error) {
                    console.error(error);
                }
            });  
        }      
    });
});

function DisplayOrderCreateResult(response, isSuccess, isUserAuthorized = false) {
    if (isSuccess) {
        let message = "<p style='text-align:center'>" + response.successMsg + ", номер заказа - " + "<span style='font-size:14px;'><b>" + response.orderNumber + "</b></span>" + "</p>";
        let hestoryOfOrderBtn = "<a class'btn button-check-out' href=" + ShopUrlSettings.OrderHistory + " >Показать историю</a>"

        $("#modalBody").html(message);
        $("#BoostrapModal").addClass("fixWindowModal");
        $("#BoostrapModal").modal('show');
    }   
}