import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType  } from '@aspnet/signalr';
import { environment } from '../../environments/environment';
import { ChatMessage } from 'src/entities/ChatMessage';
import { MessagesService } from '../api/messages.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  private hubConnection : HubConnection;
  nick = '';
  message = '';
  messages: ChatMessage[] = [];
  title = 'ChatroomWeb';

  constructor(private messageService: MessagesService){ }

  ngOnInit() {
    this.nick = localStorage.getItem("user");

    this.messageService.getLastMessages().subscribe
    (
      (result:ChatMessage[]) => this.messages = result
    );

    this.hubConnection = new HubConnectionBuilder().withUrl(`${environment.apiUrl}/chat`, {
      skipNegotiation: true,
      transport: HttpTransportType.WebSockets      
    }).build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
    
    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string, timestamp: number) => {
      let chatMessage = new ChatMessage();
      chatMessage.nick = nick;
      chatMessage.message = receivedMessage;
      chatMessage.timestamp = timestamp;
      this.messages.push(chatMessage);      
    });
  }

  public sendMessage(): void {
    this.messageService.sendMessage(this.nick, this.message).subscribe(
      () => { this.message = ''; }
      , (err: any) => { console.error(err) }
      );
  }

  public getMessages(): ChatMessage[]
  {
    this.messages = this.messages
      .sort((m1, m2) => { if (m1.timestamp >= m2.timestamp) return 1; else return -1;})
      .slice(Math.max(this.messages.length - environment.maxMessages, 0));
    
    return this.messages;
  }

}
