import { ChatMessage } from './chat-message.model';

export class Chat {
    id: number;
    createdDate: Date;
    timeToCloseChat: Date;
    sessionOrUserId: number;
    isUserAuthorized: boolean;
    userEmail: string;
    isChatEnded: boolean;
    shouldBeClosed: boolean = false;
    chatMessages: Array<ChatMessage> = new Array();
}
