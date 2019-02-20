import { Component, OnInit, Input } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  private _hubConnection: HubConnection | undefined;
  public async: any;
  message = '';
  messages: string[] = [];
  @Input() userName: string;

  constructor() {
  }

  public sendMessage(): void {
    const data = `${this.userName}: ${this.message}`;

    if (this._hubConnection) {
      this._hubConnection.invoke('Send', data);
    }
  }

  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/loopy')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().then(() => console.error('Error'));

    this._hubConnection.on('Send', (data: string) => {
      const received = `${data}`;
      this.messages.push(received);
    });
  }
}
