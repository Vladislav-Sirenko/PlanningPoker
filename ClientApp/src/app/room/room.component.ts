import { Component, OnInit, Inject } from '@angular/core';
import { Cards } from '../cardMock';
import { UserService } from '../user.service';
import { User } from '../user.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent {
  cards = Cards;

  public users: User[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<User[]>(baseUrl + 'api/Users/GetUsers').subscribe(result => {
      for (const user in result) {
        if (user) {
          this.users.push(result[user]);
        }
      }
      console.log(this.users);
    }, error => console.error(error));
  }
}
