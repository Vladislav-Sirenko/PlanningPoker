import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UserVote } from '../userVote.model';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
  @Input() value: number;
  // tslint:disable-next-line:no-output-on-prefix
  @Output() onChanged = new EventEmitter<number>();
  constructor() { }

  ngOnInit() {
  }


  getHeaderClass() {

    return this.checkUserVote() ? 'card border-success  mb-3 bg-success' :
      'card border-success  mb-3 ';
  }
  getBodyClass() {

    return this.checkUserVote() ? 'card-body bg-success' : 'card-body ';
  }
  checkUserVote(): boolean {
    if (this.value) {
      return sessionStorage.getItem('UserVote') === this.value.toString();
    }
    return false;
  }

  public sendMessage() {
    if (!sessionStorage.getItem('UserVote')) {
      this.onChanged.emit(this.value);
    }
  }

}
