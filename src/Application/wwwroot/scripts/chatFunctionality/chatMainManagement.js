var msgManager = messageManagement();
var coockieManager = coockieManagement();
var chatBody = $('#chatBody');
var chatWindow = $('#chatWindow');
var _chatId;
var coundownInactiveChat;
var messagePolling;
var messageObject = {

    ChatId: _chatId,

    CreatedDate: null,

    Message: null
};
var messages = [];


var createChat = function () {
    $.ajax({
        type: "GET",
        url: ShopUrlSettings.CreateChat,
        success: function (result) {
            _chatId = result.id;
            chatBody.append(msgManager.appendMessage(result.chatMessages, result.userName));
            messagePolling = window.setInterval(function () { msgManager.messageServerPoll(messages); }, 5000);
            coundownInactiveChat = setTimeout(msgManager.announceChatWillBeEnded, msgManager.stopChatAfter);
            $('#chatStartedTime').text(moment().format('HH:mm'));
        },
        error: function (error) {
            console.error(error);
        }
    });
};

//Chat window management section

var confirmCloseChat = function () {
    $("#modalHeader").toggleClass("text-center");
    $("#modalHeader").html("<h3>Вы уверены что хотите закрыть чат?</h3>");
    $('#modalBody').html('<div class="text-center"><div class="btn-group"><button class="btn btn-danger"' +
        ' onClick="closeChat()">Закрыть чать</button>' +
        '<button class="btn btn-primary" onClick="cancelCloseChat()">Отмена</button></div></div>');
    $(".modal-footer").css({ "border": "0" });
    $("#close-modal-dialog").css({ "top": "20%", "width": "30%" });
    $("#BoostrapModal").modal("show");
    $('#cancelClose').width($('#confirmClose').width());

    $('#body').addClass("fixBodyModal");
};

var closeChat = function () {
    $("#BoostrapModal").modal("hide");
    $("#chatWindow").toggle();
    $("#chatButton").toggle();
    chatBody.empty();
    msgManager.msgContentInput.empty();
    msgManager.msgContentInput.val('');
    $('#body').removeClass("fixBodyModal");

    $.ajax({
        type: "POST",
        url: "/api/CloseChat/" + _chatId,
        success: function (result) {
            if (result) {
                coockieManager.deleteCookie('BD_Chat');
                //Clear all interval
                for (var i = 1; i < 99999; i++) {
                    clearInterval(i);
                }
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
    
};

var cancelCloseChat = function () {
    //Clear all intervals
    for (var i = 1; i < 99999; i++) {
        clearInterval(i);
    }

    coundownInactiveChat = setTimeout(msgManager.announceChatWillBeEnded, msgManager.stopChatAfter);
    $("#BoostrapModal").modal("hide");
    $('#body').removeClass("fixBodyModal");
};

$("#chatButton").click(function () {
    $("#chatWindow").toggle();
    $("#chatButton").toggle();
    createChat();
});

$("#closeChat").click(function () {
    confirmCloseChat();
});

$('#collapseChat').click(function () {
    if (chatWindow) {
        $('#panelChat').toggle();
        chatWindow.toggleClass('chat-frame-collapsed');
        $('#collapseChat').css({ color: 'white' });
    }
});

msgManager.msgContentInput.on('input', function (event) {
    if (coundownInactiveChat) {
        clearInterval(coundownInactiveChat);
        coundownInactiveChat = null;
    }
});

//End of Chat window management section

