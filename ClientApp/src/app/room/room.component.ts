import { Component, OnInit, Inject, Input } from '@angular/core';
import { Cards } from '../cardMock';
import { UserService } from '../user.service';
import { Subject } from 'rxjs/Subject';
import { takeUntil } from 'rxjs/operators';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { UserVote } from '../userVote.model';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {

  @Input() userName: string;
  cards = Cards;
  userVote: UserVote[] = [];
  Votes: string[] = [];
  private subscribeUntil$: Subject<any>;
  public users: string[] = [];
  private _hubConnection: HubConnection | undefined;
  ngOnInit(): void {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/loopy')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().then(() => { });

    this._hubConnection.on('Connect', (data: string) => {
      const received = `${data}`;
      this.users.push(received);
    });
    this._hubConnection.on('Disconnect', () => {
      console.log(this.userName);
      this.userService.deleteUser(this.userName);
    });
    this.userService.cleared.pipe(takeUntil(this.subscribeUntil$))
      .subscribe(
        () => {
          this.users = this.userService.getUsers();
        });
  }

  // tslint:disable-next-line:use-life-cycle-interface
  ngOnDestroy() {
    this.subscribeUntil$.next();
    this.subscribeUntil$.complete();
  }

  onChanged(number: number) {
    const userName = localStorage.getItem('UserName');
    this.userService.addUserVote(userName, number);
    const data = `${userName} voted`;
    if (this._hubConnection) {
      this._hubConnection.invoke('Vote', data);
    }
  }

  getVotes() {
     this.userService.getUserVote().subscribe(result => {
      for (const user in result) {
        if (user) {
          this.Votes.push(user);
          this.userVote.push(result[user]);
             }
      }
    }, error => console.error(error));
    console.log(this.userVote);
  }

  resetVotes() {
    this.userVote = [];
    this.Votes = [];
    this.userService.resetUserVotes();
 }


  constructor(private userService: UserService) {
    this.users = userService.getUsers();
  }
}
