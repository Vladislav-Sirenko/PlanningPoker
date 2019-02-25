import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './user.model';
@Injectable()
export class UserService {


  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
   }
  _baseUrl: string;
  users: string [] = [];

  addUser(user: User) {
    this.http.post(this._baseUrl  + 'api/Users/AddUser', user).subscribe();
    this.users.push(user.Name);
  }
  getUsers(): string [] {
    this.http.get<User []>(this._baseUrl  + 'api/Users/GetUsers').subscribe(result => {
      for (const user in result) {
        if (user) {
          this.users.push(user);
        }
      }
    }, error => console.error(error));
    console.log(this.users);
    return this.users;
  }
}

