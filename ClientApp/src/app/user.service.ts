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
  public cleared = this._cleared.asObservable();
  private _hubConnection: signalR.HubConnection | undefined;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/loopy')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().then(() => { });
  }


  addUser(user: User) {
    this.http.post(this._baseUrl + 'api/Users/AddUser', user).subscribe();
    localStorage.setItem('UserName', user.Name);
    const data = `${user.Name}`;

    if (this._hubConnection) {
      this._hubConnection.invoke('Connect', data);
    }
    this._cleared.next();
  }
  deleteUser(user: string) {
    this.http.post(this._baseUrl + 'api/Users/DeleteUser', user).subscribe();
    localStorage.removeItem('UserName');
    this._cleared.next();
  }
  getUsers(): string[] {
    this.http.get<User[]>(this._baseUrl + 'api/Users/GetUsers').subscribe(result => {
      for (const user in result) {
        if (user) {
          this.users.push(user);
        }
      }
    }, error => console.error(error));
    console.log(this.users);
    return this.users;
  }

  addUserVote(userName: string, vote: number) {
    this.http.post(this._baseUrl + 'api/Users/Vote', { userName, vote }).subscribe();
  }

  getUserVote(): Observable<UserVote[]> {
    return this.http.get<UserVote[]>(this._baseUrl + 'api/Users/GetVotes');
  }

  resetUserVotes() {
    this.http.post(this._baseUrl + 'api/Users/ResetVotes', null).subscribe();
  }
}

