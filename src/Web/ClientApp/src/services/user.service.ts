import { Injectable } from '@angular/core';
import { Observable, of, Subject } from 'rxjs';
import { GetCurrentUserResponse, UsersClient } from '../app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private currentUser = new Subject<GetCurrentUserResponse>();

  constructor(private usersClient: UsersClient) { }

  loadCurrentUser(): void {
    this.usersClient.getCurrentUser().subscribe(
      {
        next: response => this.currentUser.next(response)
      }
    );
  }

  getCurrentUser(): Observable<GetCurrentUserResponse> {
    return this.currentUser.asObservable();
  }
}
