import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from './user.model';

@Injectable()
export class UserService {

  constructor(private http: HttpClient) { }

  users: User[] = [];

  addUser(baseUrl: string, user: User) {
    console.log(user.Name + 'FAAAAAAAAAAAA' + user.Password);
    this.http.post(baseUrl + 'api/Users/AddUser', user).subscribe();
  }
  getUsers(baseUrl: string): User[] {
    this.http.get<User[]>(baseUrl + 'api/Users/GetUsers').subscribe(result => {
      for (const user in result) {
        if (user) {
          this.users.push(result[user]);
        }
      }
    }, error => console.error(error));
    console.log(this.users);
    return this.users;
  }
}

