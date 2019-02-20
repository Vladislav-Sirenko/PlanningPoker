import { Component, OnInit, Inject, Input } from '@angular/core';
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
  @Input() userName: string;
  cards = Cards;

  public users: string[] = [];

  constructor(private userService: UserService) {
    this.users = userService.getUsers();
  }
}
