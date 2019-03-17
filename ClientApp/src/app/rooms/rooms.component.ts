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
      console.log(this.rooms);
    });
  }

  addRoom() {
    // const dialogRef = dialog.open(UserProfileComponent, {
    //   height: '400px',
    //   width: '600px',
    // });
    const room = new Room(UUID.UUID(), this.name);
    this.rooms.push(room);
    this.userService.addRoom(room);
    console.log(room);
    console.log(this.rooms);
  }
  deleteRoom(id: string) {
    const index = this.rooms.findIndex(room => room.id === id);
    this.rooms.splice(index, 1);
  }
  joinRoom(id: string, name: string) {
    this.userService.addUserToRoom(name);
    this.roouter.navigate(['room', id], { queryParams: { name: name } });
  }
  inRoom(): boolean {
  return window.location.href.includes('/room');
  }
}
