import { Component } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType  } from '@aspnet/signalr';
import { environment } from './../environments/environment';
import { ChatMessage } from 'src/entities/ChatMessage';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  private hubConnection : HubConnection;
  nick = '';
  message = '';
  messages: ChatMessage[] = [];
  title = 'ChatroomWeb';

  ngOnInit() {
    this.nick = window.prompt('Your name:', 'Pablo');

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
      this.message = '';
    });
  }

  public sendMessage(): void {
    let timestamp = Date.now()
    this.hubConnection
      .invoke('sendToAll', this.nick, this.message, timestamp)
      .catch(err => console.error(err));
  }
}
