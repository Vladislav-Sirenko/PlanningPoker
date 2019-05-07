import { Component, OnInit, Input, Output, EventEmitter, Inject } from '@angular/core';
import { User } from '../user.model';
import { UserService } from '../user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  _baseUrl: string;

  userName: string;
  userPassword: string;
  email: string;

  // tslint:disable-next-line:no-output-on-prefix
  @Output() onChanged = new EventEmitter<string>();
  user: User;

  addUser() {
    this.user = new User(this.userName, this.userPassword, this.email);
    this.userService.addUser(this.user);
    this.onChanged.emit(this.user.name);
  }

  constructor(private userService: UserService) {
  }

  ngOnInit() {
  }

}
