import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  userName: string;

  ngOnInit(): void {
    this.userName = localStorage.getItem('UserName');
  }


  onChanged(userName: any) {
  this.userName = userName;
  }
}
