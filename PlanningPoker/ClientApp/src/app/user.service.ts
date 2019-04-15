import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from './user.model';
import * as signalR from '@aspnet/signalr';
import { Subject } from 'rxjs/Subject';
import { UserVote } from './userVote.model';
import { Observable } from 'rxjs/Observable';
import { Room } from './rooms/room.model';
@Injectable()
export class UserService {

  _baseUrl: string;
  userVote = {};
  users: string[] = [];
  private _cleared = new Subject<void>();
  private _changed = new Subject<string>();
  private _disconnected = new Subject<string>();
  private _join = new Subject<string>();
  private _voted = new Subject<string>();
  private _roomChanged = new Subject<void>();
  private _finishVoting = new Subject<void>();
  private _send = new Subject<string>();
  private _roles = new Subject<string>();
  private _userDisconnect = new Subject<void>();
  private _roomRoles = new Subject<void>();
  public roles = this._roles.asObservable();
  public cleared = this._cleared.asObservable();
  public changed = this._changed.asObservable();
  public userDisconnect = this._userDisconnect.asObservable();
  public roomsChanged = this._roomChanged.asObservable();
  public send = this._send.asObservable();
  public join = this._join.asObservable();
  public voted = this._voted.asObservable();
  public disconnected = this._disconnected.asObservable();
  public finishVoting = this._finishVoting.asObservable();
  public roomRoles = this._roomRoles.asObservable();

  private _hubConnection: signalR.HubConnection | undefined;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('loopy')
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
    this._hubConnection.on('Join', (name: string) => {
      this._join.next(name);
    });
    this._hubConnection.on('AddRoom', () => {
      this._roomChanged.next();
    });
    this._hubConnection.on('DeleteRoom', () => {
      this._roomChanged.next();
    });
    this._hubConnection.on('GetRoles', (role: string) => {
      this._roles.next(role);
    });
    this._hubConnection.on('UserDisconnect', () => {
      this._userDisconnect.next();
    });
    this._hubConnection.on('GetRolesForRoom', () => {
      this._roomRoles.next();
    });
  }


  addUser(user: User) {
    localStorage.setItem('UserName', user.Name);
    const data = `${user.Name}`;
    if (this._hubConnection) {
      this._hubConnection.invoke('Connect', data);
    }
  }
  deleteUser(user: string) {
    localStorage.removeItem('UserName');
    localStorage.removeItem('UserVote');
    if (this._hubConnection) {
      this._hubConnection.invoke('Disconnect', user);
    }
  }
  getUsers(id: string) {
    return this.http.get<string[]>(this._baseUrl + 'api/Rooms/' + id + '/Users');
  }

  getUserVote(id: string): Observable<UserVote[]> {
    return this.http.get<UserVote[]>(this._baseUrl + 'api/Rooms/' + id + '/Votes');
  }

  finishVote() {
    if (this._hubConnection) {
      this._hubConnection.invoke('GetVotes');
    }
  }

  resetUserVotes(id: string) {
    this.http.post(this._baseUrl + 'api/Rooms/' + id + '/ResetVotes', id).subscribe();
    if (this._hubConnection) {
      this._hubConnection.invoke('ResetVotes');
    }
  }
  sendMessage(data: string) {
    if (this._hubConnection) {
      this._hubConnection.invoke('Send', data);
    }
  }
  addUserToRoom(roomName: string) {
    if (this._hubConnection) {
      this._hubConnection.invoke('Join', roomName);
    }
  }

  addRoom(room: Room) {
    const user = localStorage.getItem('UserName');
    room.CreatorId = user;
    this.http.post(this._baseUrl + 'api/Rooms', room).subscribe();
  }
  getRooms() {
    return this.http.get<Room[]>(this._baseUrl + 'api/Rooms');
  }
  getRoles(id: string) {
    if (this._hubConnection) {
      this._hubConnection.invoke('GetRole', id);
    }
  }
  deleteRoom(id: string) {
    return this.http.delete(this._baseUrl + 'api/Rooms/' + id).subscribe();
  }

  addUserVote(userName: string, vote: number) {
    const data = `${userName} voted`;
    if (this._hubConnection) {
      this._hubConnection.invoke('Vote', data);
    }
    this.http.post(this._baseUrl + 'api/Rooms/UserVote', { userName, vote }).subscribe();
  }
  logOut() {
    if (this._hubConnection) {
      this._hubConnection.invoke('UserDisconnected');
    }
  }
  getRolesForRoom(users: string[], id) {
    return this.http.post(this._baseUrl + 'api/Rooms/' + id + '/Roles', users);
  }
}



