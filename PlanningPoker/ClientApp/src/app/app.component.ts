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
    this.userName = sessionStorage.getItem('UserName');
    this.userService.userDisconnect.subscribe(() => {
       this.userName = null;
    });
  }


  onChanged(userName: any) {
    this.userName = userName;
  }
}
