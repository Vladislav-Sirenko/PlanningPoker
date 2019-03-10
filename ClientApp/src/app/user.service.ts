import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './user.model';
import * as signalR from '@aspnet/signalr';
import { Subject } from 'rxjs/Subject';
import { UserVote } from './userVote.model';
import { Observable } from 'rxjs/Observable';
@Injectable()
export class UserService {

  _baseUrl: string;
  userVote = {};
  users: string[] = [];
  private _cleared = new Subject<void>();
  private _changed = new Subject<string>();
  private _disconnected = new Subject<string>();
  private _voted = new Subject<string>();
  private _finishVoting = new Subject<void>();
  private _send = new Subject<string>();
  public cleared = this._cleared.asObservable();
  public changed = this._changed.asObservable();
  public send = this._send.asObservable();
  public voted = this._voted.asObservable();
  public disconnected = this._disconnected.asObservable();
  public finishVoting = this._finishVoting.asObservable();
  private _hubConnection: signalR.HubConnection | undefined;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/loopy')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().then(() => { });
    this._hubConnection.on('Connect', (data: string) => {
      const received = `${data}`;
      this.users.push(received);
      this._changed.next(data);
    });
    this._hubConnection.on('ResetVotes', () => {
      this._cleared.next();
    });
    this._hubConnection.on('Disconnect', (name: string) => {
      this._disconnected.next(name);
    });
    this._hubConnection.on('Vote', (name: string) => {
      this._voted.next(name);
    });
    this._hubConnection.on('Send', (name: string) => {
      this._send.next(name);
    });
    this._hubConnection.on('GetVotes', () => {
      this._finishVoting.next();
    });
  }


  addUser(user: User) {
    this.http.post(this._baseUrl + 'api/Users/AddUser', user).subscribe();
    localStorage.setItem('UserName', user.Name);
    const data = `${user.Name}`;
    if (this._hubConnection) {
      this._hubConnection.invoke('Connect', data);
    }
  }
  deleteUser(user: string) {
    localStorage.removeItem('UserName');
    if (this._hubConnection) {
      this._hubConnection.invoke('Disconnect', user);
    }
  }
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this._baseUrl + 'api/Users/GetUsers');
  }

  addUserVote(userName: string, vote: number) {
    const data = `${userName} voted`;
    if (this._hubConnection) {
      this._hubConnection.invoke('Vote', data);
    }
    this.http.post(this._baseUrl + 'api/Users/Vote', { userName, vote }).subscribe();
  }

  getUserVote(): Observable<UserVote[]> {
    return this.http.get<UserVote[]>(this._baseUrl + 'api/Users/GetVotes');
  }

  finishVote() {
    if (this._hubConnection) {
      this._hubConnection.invoke('GetVotes');
    }
  }

  resetUserVotes() {
    this.http.post(this._baseUrl + 'api/Users/ResetVotes', null).subscribe();
    if (this._hubConnection) {
      this._hubConnection.invoke('ResetVotes');
    }
  }
  sendMessage(data: string) {
    if (this._hubConnection) {
      this._hubConnection.invoke('Send', data);
    }
  }
}

