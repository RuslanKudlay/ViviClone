function messageManagement() {

    var stopChatAfter = 2 * 60000,
        coockieExpire = 2,
        coundownInactiveChat,
        msgContent;

    var sendMsgButton = $('#sendMsg');
    var msgContentInput = $('#msgContent');

    var messageServerPoll = function (messages) {
        if (messages.length > 0) {
            var queryMessage = {
                chatId: _chatId,
                LastMsgDate: messages[messages.length - 1].LastMsgDate
            };
            $.ajax({
                type: "POST",
                data: JSON.stringify([queryMessage]),
                contentType: "application/json; charset=utf-8",
                url: "/api/Chat/NewestMsg",
                success: function (result) {
                    if (result && result.length > 0) {
                        if (result[0].endedByOperator) {
                            chatBody.append('<div style="width: 100%; color: red">Чат завершен по инициативе оператора!!!</div>');
                            chatBody.animate({ scrollTop: chatBody.height() }, 'slow');
                            coockieManager.deleteCookie('BD_Chat');
                            //Stop server polling
                            for (var i = 1; i < 99999; i++) {
                                clearInterval(i);
                            }
                        }
                        else if (!result[0].chatShouldBeClosed) {
                            chatBody.append(appendMessage(result, null));
                            chatBody.animate({ scrollTop: chatBody.height() }, 'slow');
                        }
                    }
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    };

    var appendMessage = function (messageContent, userName) {
        var name;
        userName !== undefined && userName !== null ? name = userName : name = "";
        var messageRenderer = "";
        if (!messageContent || messageContent.length === 0) {
            return '<div class="speech" style="margin-bottom: -3%;">' + 'Здравствуйте ' + name + ' , чем можем Вам помочь? ' + '</div>' +
                '<i style="font-size: x-small;" >' + moment().format('DD/MM/YYYY HH:mm') + '</i >';
        }
        else {
            messageContent.forEach(function (item, i, messageContent) {
                if (item.isMessageFromOperator) {
                    messageRenderer += '<div class="speech" style="margin-bottom: -3%;">' + item.message + '</div>' +
                        '<i style="font-size: x-small;" >' + moment(item.createdDate).format('DD/MM/YYYY HH:mm') + '</i >';
                }
                else {
                    messageRenderer += '<div class="speech" style="margin-left:auto; margin-right:0; margin-bottom: -3%;">'
                        + item.message + '</div>' +
                        '<i style="font-size: x-small; margin-left: 70%;" >' + moment(item.createdDate).format('DD/MM/YYYY HH:mm') + '</i >';
                }
                messageObject.LastMsgDate = item.createdDate;
                messages.push(messageObject);
            });
            return messageRenderer;
        }
    };

    var sendMessage = function (_message) {

        $.ajax({
            type: "POST",
            data: JSON.stringify(messageObject),
            contentType: "application/json; charset=utf-8",
            url: "/api/Chat/AddMessage",
            success: function (result) {
                chatBody.append('<div class="speech" style="margin-left:auto; margin-right:0; margin-bottom: -3%;">' + msgContent + '</div>');
                chatBody.append('<i style="font-size: x-small; margin-left: 70%;" >' + moment(result.createdDate).format('DD/MM/YYYY HH:mm') + '</i>');
                
                chatBody.animate({ scrollTop: chatBody.height() }, 'slow');
                msgContentInput.val('');
                messageObject.Message = msgContent;
                messageObject.ChatId = _chatId;

                sendMsgButton.prop("disabled", true);
                msgContent = "";

                message = result;
                messageObject.LastMsgDate = result.createdDate;
                messages.push(messageObject);
                coockieManager.setCookie('BD_Chat', null, { expires: coockieExpire });
            },
            error: function (error) {
                console.error(error);
            }
        });
    };

    var formTheMessageObject = function () {
        messageObject.Message = msgContent;
        messageObject.ChatId = _chatId;
    };

    sendMsgButton.click(function () {
        setCoundownInactiveChat();
        formTheMessageObject();
        sendMessage(messageObject);
    });

    msgContentInput.on('input', function (event) {
        msgContent = this.value.replace(/\n/gi, '<br />');
        if (msgContent && msgContent !== '') {
            sendMsgButton.prop("disabled", false);
        }
        else {
            sendMsgButton.prop("disabled", true);
        }
        setCoundownInactiveChat();
    });

    msgContentInput.keydown(function (event) {
        if (event.keyCode === 13 && !event.shiftKey) {
            event.preventDefault();
            if (msgContent && msgContent !== '') {
                sendMsgButton.trigger('click');
            }
        }
    });

    msgContentInput.keyup(function (event) {
        if (event.keyCode === 13 && event.shiftKey) {
            msgContent = this.value.replace(/\n/gi, '<br />');
        }
    });

    var announceChatWillBeEnded = function () {
        $("#modalHeader").toggleClass("text-center");
        $("#modalHeader").html("<h3>Ваш чат был неактивен какое-то время.</h3>");
        $('#modalBody').html('<div class="text-center">Автозавершение чата через <span id="timeTillClose">10</span> секунд!</div>' +
            '<div class="text-center"><div class="btn-group">' +
            '<button class="btn btn-primary" onClick="cancelCloseChat();">Отмена</button></div></div>');
        $(".modal-footer").css({ "border": "0" });
        $(".modal-dialog").css({ "top": "20%", "width": "30%" });
        $("#BoostrapModal").modal("show");
        $('#cancelClose').width($('#confirmClose').width());
        chatClosingTimer(10, '#timeTillClose');
    };

    var chatClosingTimer = function (duration, display) {
        var timer = duration;
        var timerId = setInterval(function () {
            $(display).text(timer);
            if (--timer < 0) {
                clearInterval(timerId);
                closeChat();
            }
        }, 1000);
    };
       
    var setCoundownInactiveChat = function () {
        if (coundownInactiveChat) {
            clearTimeout(coundownInactiveChat);
        }
        coundownInactiveChat = setTimeout(announceChatWillBeEnded, stopChatAfter);
    }

    return {
        messageServerPoll: messageServerPoll,
        appendMessage: appendMessage,
        sendMessage: sendMessage,
        announceChatWillBeEnded: announceChatWillBeEnded,
        stopChatAfter: stopChatAfter,
        msgContentInput: msgContentInput
    };
}

