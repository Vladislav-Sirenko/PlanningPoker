import { Component, OnInit, Inject, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { Cards } from '../cardMock';
import { UserService } from '../user.service';
import { UserVote } from '../userVote.model';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../user.model';
import { ToastService } from '../toast/toast.service';
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
  Votes: string[] = [];
  role: string;
  users: User[] = [];
  id: string;
  roles: string[] = [];
  creatorName: string;
  sessionEnded = false;
  @HostListener('window:beforeunload') goToPage() {
    this.userService.deleteUserFromRoom();
    sessionStorage.setItem('Unload', '1');
  }
  ngOnInit(): void {

    sessionStorage.removeItem('UserVote');
    if (sessionStorage.getItem('Unload')) {
      this.router.navigate(['']);
      sessionStorage.removeItem('Unload');
    }
    if (sessionStorage.getItem('UserName').toLowerCase() === this.creatorName.toLowerCase()) {
      this.role = 'Admin';
    } else { this.role = 'Guest'; }
    this.votesCount = 0;
    this.userService.join
      .subscribe(
        (roomState: boolean) => {
          this.sessionEnded = roomState;
          this.userService.getUsers(this.id).subscribe(users => {
            this.users = users;
            for (const user of this.users) {
              this.roles[this.users.indexOf(user)] = 'Guest';
              if (user.vote) {
                this.Votes[this.users.indexOf(user)] = '✔';
              } else { this.Votes[this.users.indexOf(user)] = ''; }
              if (user.name.toLowerCase() === this.creatorName.toLowerCase()) {
                this.roles[this.users.indexOf(user)] = 'Admin';
              }
            }
            if (sessionStorage.getItem('UserName').toLowerCase() === this.creatorName.toLowerCase()) {
              this.userService.notifyAdminRole();
            }
          }, err => {
            this.toaster.error('Something went wrong ' + err);
          });
        });
    this.userService.adminJoined.subscribe(name => {
      this.toaster.success(name + ' joined room as Admin');
      const user = this.users.find(x => x.name.toLowerCase() === name.toLowerCase());
      const index = this.users.indexOf(user);
      this.roles[index] = ' Admin';
    });
    this.userService.voted.subscribe((name) => {
      const user = this.users.find(x => x.name.toLowerCase() === name.toLowerCase());
      this.Votes[this.users.indexOf(user)] = '✔';
    });
    this.userService.finishVoting
      .subscribe(result => {
        this.toaster.success(' Voting Finished');
        this.sessionEnded = true;
        for (const name in result) {
          if (name) {
            const user = this.users.find(x => x.name.toLowerCase() === name.toLowerCase());
            if (user) {
              user.vote = result[name];
            }
          }
        }
      },
        err => {
          this.toaster.error('Something went wrong ' + err);
        });
    this.userService.disconnected
      .subscribe(
        (name) => {
          const user = this.users.find(x => x.name.toLowerCase() === name.toLowerCase());
          const index = this.users.indexOf(user);
          if (this.users.includes(user)) {
            this.roles.splice(index, 1);
            this.Votes.splice(index, 1);
            this.users.splice(index, 1);
          }
        });
    this.userService.cleared
      .subscribe(
        () => {
          this.toaster.success(' Voting reseted');
          this.Votes = [];
          this.votesCount = 0;
          this.sessionEnded = false;
          sessionStorage.removeItem('UserVote');
        });
    this.userService.roomDeleted.subscribe(() => {
      sessionStorage.removeItem('UserVote');
      this.router.navigate(['']);
    });
  }

  isAdmin(): boolean {
    return this.role === 'Admin';
  }


  onChanged(number: number) {
    sessionStorage.setItem('UserVote', number.toString());
    this.userService.addUserVote(number);
  }

  getVotes() {
    this.userService.getUserVote(this.id).subscribe();
  }

  resetVotes() {
    sessionStorage.removeItem('UserVote');
    for (const user of this.users) {
      user.vote = null;
    }
    this.userService.resetUserVotes(this.id);
  }

  logOut() {
    sessionStorage.removeItem('UserVote');
    this.userService.deleteUserFromRoom();
    this.users = [];
    this.roles = [];
    this.router.navigate(['']);
  }

  buttonDisabled(): boolean {
    if (this.votesCount < this.users.length) {
      return true;
    }
    return false;
  }
  showVote(user: User) {
    return this.sessionEnded === true ? user.vote : '';
  }

  constructor(private userService: UserService, private router: Router, private route: ActivatedRoute, private toaster: ToastService) {
    this.route.queryParams.subscribe(params => {
      this.name = params['name'];
      this.id = params['id'];
      this.creatorName = params['creatorName'];
    });
  }
}
