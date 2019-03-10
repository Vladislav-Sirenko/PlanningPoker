import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { Cards } from '../cardMock';
import { UserService } from '../user.service';
import { Subject } from 'rxjs/Subject';
import { UserVote } from '../userVote.model';
@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {

  @Input() userName: string;
  @Output() logout = new EventEmitter<string>();
  cards = Cards;
  userVote: UserVote[] = [];
  Votes: string[] = [];
  private subscribeUntil$: Subject<any>;
  public users: string[] = [];
  ngOnInit(): void {
    this.userService.getUsers().subscribe(users => {
      // tslint:disable-next-line:forin
      for (const user in users) {
        this.users.push(user);
      }
    });
    this.userService.finishVoting
  .subscribe(() => {
    this.userService.getUserVote().subscribe(result => {
      this.Votes = [];
      this.userVote = [];
      for (const user in result) {
        if (user) {
          this.Votes.push(user);
          this.userVote.push(result[user]);
        }
      }
    }, error => console.error(error));
    console.log(this.userVote);
  });
this.userService.cleared
  .subscribe(
    () => {
      this.Votes = [];
      this.userVote = [];
    });
this.userService.changed
  .subscribe(
    () => {
      this.userService.getUsers().subscribe(users => {
        this.users = [];
        // tslint:disable-next-line:forin
        for (const user in users) {
          this.users.push(user);
        }
      });
    });
this.userService.disconnected
  .subscribe(
    () => {
      this.userService.getUsers().subscribe(users => {
        this.users = [];
        // tslint:disable-next-line:forin
        for (const user in users) {
          this.users.push(user);
        }
      });
    });
  }

// tslint:disable-next-line:use-life-cycle-interface
ngOnDestroy() {
  this.subscribeUntil$.next();
  this.subscribeUntil$.complete();
}

onChanged(number: number) {
  const userName = localStorage.getItem('UserName');
  localStorage.setItem('UserVote', number.toString());
  this.userService.addUserVote(userName, number);
}

getVotes() {
  this.userService.finishVote();
}

resetVotes() {
  localStorage.removeItem('UserVote');
  this.userService.resetUserVotes();
}

logOut() {
  const userName = localStorage.getItem('UserName');
  this.userService.deleteUser(userName);
}


constructor(private userService: UserService) {
  this.userService.getUsers().subscribe(users => {
    // tslint:disable-next-line:forin
    for (const user in users) {
      this.users.push(user);
    }
  });
}
}
