import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  errorMessage: string | null = null;

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.notificationService.error$.subscribe((message) => {
      this.errorMessage = message;
    });
  }

  onCloseErrorClick(): void {
    this.notificationService.clearError();
  }
}
