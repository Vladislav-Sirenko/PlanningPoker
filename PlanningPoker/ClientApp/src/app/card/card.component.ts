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

    return localStorage.getItem('UserVote') === this.value.toString() ? 'card border-success  mb-3 bg-success' :
      'card border-success  mb-3 ';
  }
  getBodyClass() {

    return localStorage.getItem('UserVote') === this.value.toString() ? 'card-body bg-success' : 'card-body ';
  }


  public sendMessage() {
    if (!localStorage.getItem('UserVote')) {
      this.onChanged.emit(this.value);
    }
  }

}
