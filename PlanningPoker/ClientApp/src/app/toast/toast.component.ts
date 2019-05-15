import { Component, OnInit } from '@angular/core';
import { Toast, ToastPackage, ToastrService } from 'ngx-toastr';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-toast',
  template: `
  <button *ngIf="options.closeButton" (click)="remove()" class="toast-close-button autosize" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
  <div *ngIf="title" [class]="options.titleClass" [attr.aria-label]="title">
    {{ title }}
  </div>
  <div *ngIf="message && options.enableHtml" role="alertdialog" aria-live="polite"
    [class]="options.messageClass" [innerHTML]="message">
  </div>
  <span *ngIf="message && !options.enableHtml" role="alertdialog" aria-live="polite"
    [class]="options.messageClass" [attr.aria-label]="message">
    {{ message }}
  </span>
  <div *ngIf="options.progressBar">
    <div class="toast-progress" [style.width]="width + '%'"></div>
  </div>
  `,
  animations: [
    trigger('flyInOut', [
      state(
        'inactive',
        style({
          display: 'none',
          opacity: 0
        })
      ),
      state('active', style({})),
      state('removed', style({ opacity: 0 })),
      transition('inactive => active', [
          style({
            opacity: 0,
            transform: 'translateX(100%)',
          }),
          animate('0.2s ease-in')
        ]),
      transition('active => removed', [
        animate('0.2s 10ms ease-out'),
        style({
          opacity: 0,
          transform: 'translateX(100%)',
        })
      ]),
    ])
  ],
  preserveWhitespaces: false
})
export class ToastComponent extends Toast {

  constructor(
    protected toastrService: ToastrService,
    public toastPackage: ToastPackage,
  ) {
    super(toastrService, toastPackage);
  }

  action(event: Event) {
    event.stopPropagation();
    this.toastPackage.triggerAction();
    return false;
  }

}
