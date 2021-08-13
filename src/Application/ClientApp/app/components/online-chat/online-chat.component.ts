import { AfterViewInit, Component, OnDestroy, OnInit, QueryList, ViewChildren } from '@angular/core';
import { MessageQuery } from '../../models/message-query.model';
import { Observable } from 'rxjs';
import { interval } from 'rxjs';
import { ChatMessage } from '../../models/chat-message.model';
import { Chat } from '../../models/chat.model';
import { ChatService } from '../../services/chat.service';
import { ExtensionModalService } from '../../services/extension-modal-service';

@Component({
    templateUrl: './online-chat.component.html',
    styleUrls: ['./online-chat.component.css']
})
export class OnlineChatComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChildren('chatBody') private chatBodyScrollable: QueryList<any>;

    chatsOnline: Array<Chat> = new Array<Chat>();
    msgQueryModel: Array<MessageQuery>;
    msgPollSubscriber: any;
    chatPollSubscriber: any;
    supportMessage: ChatMessage;
    SERVER_POLL_FREQUENCY: number = 4000;

    constructor(private chatService: ChatService, private closeChatModal: ExtensionModalService) {
        if (this.chatService.chatsOnline && this.chatService.chatsOnline.length > 0) {
            this.chatsOnline = this.chatService.chatsOnline;
        }
        this.chatService.isChatPageActive = true;
    }

    public ngOnInit() {

    }

    public ngAfterViewInit() {

        this.chatPollSubscriber = interval(this.SERVER_POLL_FREQUENCY)
            .subscribe(() => { this.chatServerPoll(); });
        this.msgPollSubscriber = interval(this.SERVER_POLL_FREQUENCY)
            .subscribe(() => { this.messageServerPoll(); });
        this.scrollChatBody();
    }

    public ngOnDestroy() {
        this.chatPollSubscriber.unsubscribe();
        this.msgPollSubscriber.unsubscribe();
        this.chatService.isChatPageActive = false;
    }

    public chatServerPoll() {
        this.chatService.getNewChats(this.chatsOnline).subscribe((response) => {
            if (response && response.length > 0) {
                response.forEach(element => {
                    const index = this.chatsOnline.findIndex(chOnline => chOnline.id == element.id);
                    if (index == -1) {
                        this.chatsOnline.push(element);
                    } else {
                        if (element.isChatEnded) {
                            this.chatsOnline.find(ch => ch.id == element.id).shouldBeClosed = true;
                        }
                    }
                });
                this.markEndedChat(this.chatsOnline);
                this.chatService.chatsOnline = this.chatsOnline;
                this.markInactiveChat(this.chatsOnline);
            } else {
                this.markInactiveChat(this.chatsOnline);
            }
        }, (error: any) => {
            alert(error.message);
        });
    }

    public messageServerPoll() {
        this.configMsgQuery(this.chatsOnline);
        if (this.msgQueryModel.length > 0) {
            this.chatService.getNewestMsg(this.msgQueryModel).subscribe((response) => {
                if (response && response.length > 0) {
                    response.forEach(element => {
                        if (element.chatShouldBeClosed) {
                            this.chatsOnline.find(ch => ch.id == element.chatId).shouldBeClosed = true;
                        } else {
                            const concreteChat = this.chatsOnline.find(ch => ch.id == element.chatId);
                            if (concreteChat) {
                                concreteChat.chatMessages.push(element);
                            }
                        }
                    });
                }
            }, (error: any) => {
                alert(error.error);
            });
        }
    }

    public sendMessage(_message: string, _chatId: number) {
        this.supportMessage = {
            id: 0,
            chatId: _chatId,
            message: this.replaceNewLine(_message),
            isMessageFromOperator: true,
            createdDate: null
        };

        this.chatService.addNewMessage(this.supportMessage).subscribe((response) => {
            if (response) {
                this.chatsOnline.find(ch => ch.id == _chatId).chatMessages.push(response);
            }
            // this.scrollChatBody();
            this.supportMessage = null;
        });
    }

    private replaceNewLine(message) {
        return message.replace(/\n/gi, '<br />');
    }

    public keyDownHandler(event, chatId) {
        if (event.keyCode == 13 && !event.shiftKey) {
            event.preventDefault();
            if (event.target.value) {
                this.sendMessage(event.target.value, chatId);
            }
        }
    }

    public keyUpHandler(event) {
        if (event.keyCode == 13 && event.shiftKey) {
        }
    }

    private configMsgQuery(chatsOnline: Array<Chat>) {
        this.msgQueryModel = new Array<MessageQuery>();
        chatsOnline.forEach(element => {
            const msgQuery = new MessageQuery();
            msgQuery.chatId = element.id;
            if (element.chatMessages.length > 0) {
                msgQuery.LastMsgDate = element.chatMessages[element.chatMessages.length - 1].createdDate;
            } else {
                msgQuery.LastMsgDate = element.createdDate;
            }
            this.msgQueryModel.push(msgQuery);
        });

        return this.msgQueryModel;
    }

    private scrollChatBody() {
        this.chatBodyScrollable.first.nativeElement.scrollTop = this.chatBodyScrollable.first.nativeElement.scrollHeight;
    }

    private markInactiveChat(chatsOnline: Array<Chat>) {
        chatsOnline.forEach(element => {
            const timeToCloseChat = new Date(element.timeToCloseChat);
            if (element.chatMessages && element.chatMessages.length > 0) {
                const onlyUserMessages = element.chatMessages.filter(msg => !msg.isMessageFromOperator);
                const lastMsgTime = new Date(onlyUserMessages[onlyUserMessages.length - 1].createdDate);
                if (lastMsgTime.getTime() >= timeToCloseChat.getTime()) {
                    element.shouldBeClosed = true;
                }
            } else if (new Date(element.createdDate).getTime() >= timeToCloseChat.getTime()) {
                element.shouldBeClosed = true;
            }
        });
    }

    private markEndedChat(chatsOnline: Array<Chat>) {
        chatsOnline.forEach(element => {
            if (element.isChatEnded) {
                element.shouldBeClosed = true;
            }
        });
    }

    private closeChat(chatId: number) {
        this.closeChatModal.showCloseChatModal().subscribe(result => {
            if (result) {
                this.chatService.closeChat(chatId).subscribe((response) => {
                    if (response) {
                        const closedChat = this.chatsOnline.find(ch => ch.id == chatId);
                        const index = this.chatsOnline.indexOf(closedChat, 0);
                        if (index > -1) {
                            this.chatsOnline.splice(index, 1);
                        }
                    } else {
                        alert('Server error!!! Try later.');
                    }
                });

            }
        });
    }

}
