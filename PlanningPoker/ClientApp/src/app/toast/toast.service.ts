import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ToastService {
    constructor(private toastr: ToastrService) { }

    success(message?: string, title?: string, enableHtml = false) {
        this.toastr.success(message, title, {
            enableHtml
        });
    }

    error(message?: string, title?: string, enableHtml = false) {
        this.toastr.error(message, title, {
            enableHtml
        });
    }

    info(message?: string, title?: string, enableHtml = false) {
        this.toastr.info(message, title, {
            enableHtml
        });
    }

    warning(message?: string, title?: string, enableHtml = false) {
        this.toastr.warning(message, title, {
            enableHtml
        });
    }

    clear() {
        this.toastr.clear();
    }
}
