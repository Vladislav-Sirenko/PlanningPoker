import { Component, Input, OnInit } from '@angular/core';
import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  userName: string;
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userName = localStorage.getItem('UserName');
    this.userService.disconnected.subscribe(() => {
      this.userName = localStorage.getItem('UserName');
    });
  }


  onChanged(userName: any) {
    this.userName = userName;
  }
}
