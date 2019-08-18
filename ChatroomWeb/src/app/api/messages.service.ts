import { Injectable }   from '@angular/core';
import { HttpClient, HttpHeaders }   from '@angular/common/http';
import { Observable }   from 'rxjs';
import { ChatMessage } from '../../entities/chatMessage';
import { environment } from '../../environments/environment'

@Injectable({
    providedIn: 'root',
  })
export class MessagesService {
  private serviceUrl = `${environment.apiUrl}/api/messages`;
  
  constructor(private http: HttpClient) { }
  
  getLastMessages(): Observable<ChatMessage[]> {
    let token = localStorage.getItem("jwt");

    return this.http.get<ChatMessage[]>(`${this.serviceUrl}/getLastMessages`, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + token,
        "Content-Type": "application/json"
      })});
  }

  sendMessage(nick: string, message:string) : Observable<any> {
    let timestamp = Date.now();
    let model = {
      'nick' : nick,
      'message' : message,
      'timestamp' : timestamp
     };

    let token = localStorage.getItem("jwt");
    return this.http.post(`${environment.apiUrl}/api/Messages/SendMessage`, model, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + token,
        "Content-Type": "application/json"
      })
    })
  }
}