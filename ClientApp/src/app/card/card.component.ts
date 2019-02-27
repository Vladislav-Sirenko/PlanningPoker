import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

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

  public sendMessage() {
    this.onChanged.emit(this.value);
  }

}
