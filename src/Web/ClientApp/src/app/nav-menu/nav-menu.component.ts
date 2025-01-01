import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { GetCurrentUserResponse } from '../web-api-client';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {

  currentUserResponse: GetCurrentUserResponse;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(response => this.currentUserResponse = response);
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
