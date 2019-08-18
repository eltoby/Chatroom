import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType  } from '@aspnet/signalr';
import { environment } from '../../environments/environment';
import { ChatMessage } from 'src/entities/ChatMessage';
import { HttpClient, HttpHeaders } from '@angular/common/http';

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

  constructor(private http: HttpClient){ }

  ngOnInit() {
    this.nick = localStorage.getItem("user");

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
    let timestamp = Date.now();
    let model = {
       'name' : this.nick,
       'message' : this.message,
       'timestamp' : timestamp
      };

    let token = localStorage.getItem("jwt");
    this.http.post(`${environment.apiUrl}/api/Messages/SendMessage`, model, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + token,
        "Content-Type": "application/json"
      })
    }).subscribe(response => {
    }, err => { console.error(err)
    });
    
    // this.hubConnection
    //   .invoke('sendToAll', this.nick, this.message, timestamp)
    //   .catch(err => console.error(err));
  }

  public getMessages(): ChatMessage[]
  {
    this.messages = this.messages
      .sort((m1, m2) => { if (m1.timestamp >= m2.timestamp) return 1; else return -1;})
      .slice(Math.max(this.messages.length - environment.maxMessages, 0));
    
    return this.messages;
  }

}
