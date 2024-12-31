import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../../services/notification.service';
import { handle400ErrorResponse } from './handlers/error-handler-400';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private notificationService: NotificationService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {

        let errorMessage = 'An unknown error occurred.';
        const status = error.status;

        switch (status) {
          case 400: handle400ErrorResponse(error)
            .then(str => { this.notificationService.showError(str); }); break;
          default: console.log('Unhandled Error', error);
        }

        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
