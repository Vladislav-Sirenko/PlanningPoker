import { Component, OnInit, Input } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { UserService } from '../user.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  message = '';
  messages: string[] = [];
  @Input() userName: string;

  constructor(private userService: UserService) {
  }

  public sendMessage(): void {
    this.userName = localStorage.getItem('UserName');
    const data = `${this.userName}: ${this.message}`;

    this.userService.sendMessage(data);
  }

  ngOnInit() {

    this.userService.changed.subscribe((name) => {
    const received = `${name} connected`;
    this.messages.push(received);
    });
    this.userService.voted.subscribe((name) => {
      const received = `${name}`;
      this.messages.push(received);
      });

      this.userService.send.subscribe((data) => {
        const received = `${data}`;
        this.messages.push(received);
        });
        this.userService.disconnected.subscribe((data) => {
          const received = `${data} disconnected`;
          this.messages.push(received);
          });
  }
}
