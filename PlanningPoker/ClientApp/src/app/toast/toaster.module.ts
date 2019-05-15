import { NgModule } from '@angular/core';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { ToastComponent } from './toast.component';
import { ToastService } from './toast.service';


@NgModule({
    declarations: [ToastComponent],
    entryComponents: [ToastComponent],
    imports: [
        BrowserAnimationsModule,
        ToastrModule.forRoot({
            toastComponent: ToastComponent,
            newestOnTop: true,
            closeButton: true,
            preventDuplicates: true,
            resetTimeoutOnDuplicate: true,
            autoDismiss: true,
            positionClass: 'toast-bottom-right',
            messageClass: 'ngx-toaster-message',
            titleClass: 'ngx-toaster-title',
        })],
    exports: [ ToastrModule ],
    providers: [ ToastService ]
})
export class ToasterModule { }
