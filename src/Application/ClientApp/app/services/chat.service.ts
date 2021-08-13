import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Chat } from '../models/chat.model';
import { HttpClient } from '@angular/common/http';
import { GenericRestService } from './generic.service.';
import { ChatMessage } from '../models/chat-message.model';
import { MessageQuery } from '../models/message-query.model';


@Injectable()
export class ChatService extends GenericRestService<Chat> {

    private chatQueryModel: any;

    public chatsOnline: Array<Chat>;

    public isChatPageActive: boolean = false;

    constructor(protected http: HttpClient) {
        super(http, 'api/Chat');
    }

    getNewChats(chatReceived: Array<Chat>): Observable<any> {
        this.configChatQuery(chatReceived);
        return this.http.post('api/Chat/GetNew', this.chatQueryModel, this.httpOptions);
    }

    getNewestMsg(activeChats: Array<MessageQuery>): Observable<any> {
        return this.http.post('api/Chat/NewestMsg', activeChats, this.httpOptions);
    }

    addNewMessage(message: ChatMessage): Observable<any> {
        return this.http.post('api/Chat/AddMessage', message, this.httpOptions);
    }

    closeChat(chatId: number): Observable<any> {
        return this.http.post('api/CloseChat/' + chatId, chatId, this.httpOptions);
    }

    private configChatQuery(chatsOnline: Array<Chat>) {
        this.chatQueryModel = new Array<number>();
        chatsOnline.forEach(element => {
            if (!element.isChatEnded || !element.shouldBeClosed) {
                this.chatQueryModel.push(element.id);
            }
        });

        return this.chatQueryModel;
    }

    removeEndedChat(receivedChats: Array<Chat>) {
        // receivedChats.length = 0;
        this.chatsOnline.forEach(element => {
            const alreadyReceived = receivedChats.find(ch => ch.id == element.id);
            if (!element.isChatEnded) {
                if (!alreadyReceived) {
                    receivedChats.push(element);
                }
            } else {
                const index = receivedChats.indexOf(alreadyReceived);
                if (index !== -1) {
                    receivedChats.splice(index, 1);
                }
            }
        });
    }
}
