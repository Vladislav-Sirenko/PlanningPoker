import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { Cards } from '../cardMock';
import { UserService } from '../user.service';
import { Subject } from 'rxjs/Subject';
import { UserVote } from '../userVote.model';
import { TouchSequence } from 'selenium-webdriver';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {

  @Input() userName: string;
  @Output() logout = new EventEmitter<string>();
  name: string;
  cards = Cards;
  votesCount: 0;
  userVote: UserVote[] = [];
  Votes: string[] = [];
  private subscribeUntil$: Subject<any>;
  public users: string[] = [];
  ngOnInit(): void {
    this.userService.getUsers(this.name).subscribe(users => {
      this.users = [];
      // tslint:disable-next-line:forin
      for (const user in users) {
        this.users.push(users[user]);
      }
    });
    this.userService.finishVoting
      .subscribe(() => {
        this.userService.getUserVote(this.name).subscribe(result => {
          this.userVote = [];
          for (const user in result) {
            if (user) {
              const index = this.users.indexOf(user);
              this.userVote[index] = result[user];
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
          localStorage.removeItem('UserVote');
          this.users = [];
          this.userService.getUsers(this.name).subscribe(users => {
            this.users = [];
            // tslint:disable-next-line:forin
            for (const user in users) {
              this.users.push(users[user]);
            }
          });
        });
    this.userService.changed
      .subscribe(
        () => {
          this.userService.getUsers(this.name).subscribe(users => {
            this.users = [];
            // tslint:disable-next-line:forin
            for (const user in users) {
              this.users.push(users[user]);
            }
          });
        });
    this.userService.disconnected
      .subscribe(
        (name) => {
          this.userService.getUsers(this.name).subscribe(users => {
            this.userVote[this.users.indexOf(name)] = null;
            this.Votes[this.users.indexOf(name)] = null;
            this.users = [];
            // tslint:disable-next-line:forin
            for (const user in users) {
              this.users.push(users[user]);
            }
          });
        });
    this.userService.voted.subscribe((name) => {
      this.userService.getUsers(this.name).subscribe(users => {
        this.users = [];
        // tslint:disable-next-line:forin
        for (const user in users) {
          this.users.push(users[user]);
        }
      });
      const index = this.users.indexOf(name.split(' ')[0]);
      // tslint:disable-next-line:forin
      for (const user in this.users) {
        if (this.users[user] === name.split(' ')[0]) {
          this.Votes[index] = '✔';
        } else {
          this.Votes[user] === '✔' ? this.Votes[user] = '✔' :
            this.Votes[user] = 'No';
        }
      }

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
    this.votesCount++;
    this.userService.addUserVote(userName, number);
  }

  getVotes() {
    this.userService.finishVote();
  }

  resetVotes() {
    localStorage.removeItem('UserVote');
    this.userService.resetUserVotes(this.name);
    this.votesCount = 0;
  }

  logOut() {
    const userName = localStorage.getItem('UserName');
    this.userService.deleteUser(userName);
  }

  buttonDisabled(): boolean {
    if (this.votesCount < this.users.length) {
      return true;
    }
    return false;
  }

  constructor(private userService: UserService, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.name = params['name'];
    });

    this.userService.getUsers(this.name).subscribe(users => {
      // tslint:disable-next-line:forin
      for (const user in users) {
        this.users.push(users[user]);
      }
    });
  }
}
