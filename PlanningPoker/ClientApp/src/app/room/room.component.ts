import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { Cards } from '../cardMock';
import { UserService } from '../user.service';
import { UserVote } from '../userVote.model';
import { ActivatedRoute, Router } from '@angular/router';
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
  role: string;
  public users: string[] = [];
  id: string;
  roles: string[] = [];
  ngOnInit(): void {
    this.votesCount = 0;
    this.userService.getRoles(this.id);
    this.userService.roles.subscribe((role) => {
      this.role = role;
    });
    this.userService.getUsers(this.id).subscribe(users => {
      this.users = [];
      // tslint:disable-next-line:forin
      for (const user in users) {
        this.users.push(users[user]);
      }
      this.userService.getRolesForRoom(this.users, this.id).subscribe((roles) => {
        for (const role in roles) {
          if (role) {
            this.roles.push(roles[role]);
          }
        }
      });
    });
    this.userService.finishVoting
      .subscribe(() => {
        this.userService.getUserVote(this.id).subscribe(result => {
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
          this.votesCount = 0;
          localStorage.removeItem('UserVote');
          this.users = [];
          this.userService.getUsers(this.id).subscribe(users => {
            this.users = [];
            // tslint:disable-next-line:forin
            for (const user in users) {
              this.users.push(users[user]);
            }
          });
        });
    this.userService.join
      .subscribe(
        () => {
          this.userService.getUsers(this.id).subscribe(users => {
            this.users = [];
            // tslint:disable-next-line:forin
            for (const user in users) {
              this.users.push(users[user]);
            }
            this.userService.getRolesForRoom(this.users, this.id).subscribe((roles) => {
              this.roles = [];
              for (const role in roles) {
                if (role) {
                  this.roles.push(roles[role]);
                }
              }
            });
          });
        });
    this.userService.disconnected
      .subscribe(
        (name) => {
          this.userService.getUsers(this.id).subscribe(users => {
            this.userVote[this.users.indexOf(name)] = null;
            this.Votes[this.users.indexOf(name)] = null;
            this.users = [];
            // tslint:disable-next-line:forin
            for (const user in users) {
              this.users.push(users[user]);
            }
            this.userService.getRolesForRoom(this.users, this.id).subscribe((roles) => {
              this.roles = [];
              for (const role in roles) {
                if (role) {
                  this.roles.push(roles[role]);
                }
              }
            });
          });
        });
    this.userService.voted.subscribe((name) => {
      this.userService.getUsers(this.id).subscribe(users => {
        this.users = [];
        this.votesCount++;
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
    this.userService.resetUserVotes(this.id);
  }

  logOut() {
    const userName = localStorage.getItem('UserName');
    this.userService.deleteUser(userName);
    this.router.navigate(['']);
  }

  buttonDisabled(): boolean {
    if (this.votesCount < this.users.length) {
      return true;
    }
    return false;
  }

  constructor(private userService: UserService, private router: Router, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this.name = params['name'];
      this.id = params['id'];
    });

    this.userService.getUsers(this.id).subscribe(users => {
      // tslint:disable-next-line:forin
      for (const user in users) {
        this.users.push(users[user]);
      }
    });
  }
}
