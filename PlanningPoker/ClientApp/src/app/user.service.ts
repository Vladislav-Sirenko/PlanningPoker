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
  private _finishVoting = new Subject<any>();
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
    this._hubConnection.on('Send', (data) => {
      this._send.next(data);
    });
    this._hubConnection.on('GetVotes', (votes) => {
      this._finishVoting.next(votes);
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
    sessionStorage.setItem('UserName', user.name);
    this.http.post(this._baseUrl + 'api/User', user).subscribe();
  }

  getUsers(id: string) {
    return this.http.get<User[]>(this._baseUrl + 'api/Rooms/' + id + '/Users');
  }

  getUserVote(id: string) {
    return this.http.post(this._baseUrl + 'api/Rooms/' + id + '/Votes', null);
  }

  resetUserVotes(id: string) {
    this.http.post(this._baseUrl + 'api/Rooms/' + id + '/ResetVotes', id).subscribe();
  }
  sendMessage(data: string) {
    if (this._hubConnection) {
      this._hubConnection.invoke('Send', data);
    }
  }
  addUserToRoom(id: string, userName: string) {
    if (this._hubConnection) {
      this._hubConnection.invoke('Join', id, userName);
    }
  }

  addRoom(room: Room) {
    const user = sessionStorage.getItem('UserName');
    room.creatorName = user;
    this.http.post(this._baseUrl + 'api/Rooms', room).subscribe();
  }
  getRooms() {
    return this.http.get<Room[]>(this._baseUrl + 'api/Rooms');
  }
  getRoles(id: string) {
    const userName = sessionStorage.getItem('UserName');
    let params = new HttpParams();
    params = params.append('id', id);
    params = params.append('name', userName);
    return this.http.get(this._baseUrl + 'api/Rooms/' + id + '/Role', { params: params });
  }
  deleteRoom(id: string) {
    return this.http.delete(this._baseUrl + 'api/Rooms/' + id).subscribe();
  }

  addUserVote(vote: number) {
    const id = sessionStorage.getItem('UserName');
    this.http.post(this._baseUrl + 'api/Rooms/' + id + '/UserVote', vote).subscribe();
  }
  getRolesForRoom(users: string[], id) {
    return this.http.post(this._baseUrl + 'api/Rooms/' + id + '/Roles', users);
  }

  deleteUserFromRoom() {
    const user = sessionStorage.getItem('UserName');
    this.http.delete(this._baseUrl + 'api/Rooms/DeleteUserFromRoom/' + user).subscribe();
  }
  logOut() {
    this._userDisconnect.next();
  }

}



