import { Component, OnInit, Input } from '@angular/core';
import { Room } from './room.model';
import { UUID } from 'angular2-uuid';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../user.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit {
  name: string;
  rooms: Room[] = [];
  constructor(private roouter: Router, private userService: UserService) { }
  @Input() userName: string;
  ngOnInit() {
    this.userService.getRooms().subscribe(rooms => {
      for (const room in rooms) {
        if (room) {
          this.rooms.push(new Room(rooms[room].id, rooms[room].name));
        }
      }
    });
    this.userService.roomsChanged.subscribe(() => {
      this.userService.getRooms().subscribe(rooms => {
        this.rooms = [];
        for (const room in rooms) {
          if (room) {
            this.rooms.push(rooms[room]);
          }
        }
      });
    });
  }

  addRoom() {
    const room = new Room(UUID.UUID(), this.name);
    this.userService.addRoom(room);
  }
  deleteRoom(id: string) {
    this.userService.deleteRoom(id);
  }
  joinRoom(id: string, name: string) {
    // tslint:disable-next-line:quotemark
    this.userService.addUserToRoom(id, sessionStorage.getItem("UserName"));
    this.roouter.navigate(['room', id], { queryParams: { id: id, name: name } });
  }
  inRoom(): boolean {
    return window.location.href.includes('/room');
  }
  logOut() {
    sessionStorage.removeItem('UserName');
    this.userService.logOut();
    this.roouter.navigate(['']);
  }
}
