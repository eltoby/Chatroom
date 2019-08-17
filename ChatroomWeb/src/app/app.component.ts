import { Component } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType  } from '@aspnet/signalr';
import { environment } from './../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  private hubConnection : HubConnection;
  nick = '';
  message = '';
  messages: string[] = [];
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

    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string, timestamp: string) => {
      const text = `${nick}: ${receivedMessage}`;
      this.messages.push(text);
    });
  }

  public sendMessage(): void {
    let timestamp = Date.now()
    this.hubConnection
      .invoke('sendToAll', this.nick, this.message, timestamp)
      .catch(err => console.error(err));
  }
}
