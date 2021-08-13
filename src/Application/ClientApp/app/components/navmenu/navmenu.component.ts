import { AfterViewInit, Component } from '@angular/core';
import { Chat } from '../../models/chat.model';
import { Observable } from 'rxjs';
import { ChatService } from '../../services/chat.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent implements AfterViewInit {

    chatsOnline: Array<Chat> = new Array<Chat>();
    newChatSubscriber: any;
    SERVER_POLL_FREQUENCY: number = 4000;

    constructor(
        public chatService: ChatService
    ) {
        this.chatServerPoll();
    }

    public ngAfterViewInit() {
        this.startServerPoll();
    }

    private chatServerPoll() {
        if (!this.chatService.isChatPageActive) {
            this.chatService.getNewChats(this.chatsOnline).subscribe((response) => {
                if (response && response.length > 0) {
                    this.chatService.chatsOnline = response;
                    this.chatService.removeEndedChat(this.chatsOnline);
                    this.chatService.chatsOnline = this.chatsOnline;
                }
            }, (error: any) => {
                alert(error.message);
            });
        }
    }

    private startServerPoll() {
        this.newChatSubscriber = Observable.interval(this.SERVER_POLL_FREQUENCY)
            .subscribe(() => { this.chatServerPoll(); });
    }
}
