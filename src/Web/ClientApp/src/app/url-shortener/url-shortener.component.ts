import { Component, OnInit } from '@angular/core';
import { GetCurrentUserResponse, ShortenUrlCommand, UrlsClient } from '../web-api-client';
import { UserService } from '../../services/user.service';
import { ActivatedRoute } from '@angular/router';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-url-shortener',
  templateUrl: './url-shortener.component.html',
  styleUrl: './url-shortener.component.css'
})
export class UrlShortenerComponent implements OnInit {

  url: string;
  isShortening = false;
  showResult = false;
  result: string;
  showCopiedNotification = false;
  showErrorNotification = false;
  errorMessage: string;

  generatedUrlCount = 0;

  currentUserResponse: GetCurrentUserResponse;

  showLoginSuggestionModal: boolean;
  shouldIgnoreLoginSuggestion: boolean;

  get shouldSuggestLogin() {
    const isLoggedIn = this.currentUserResponse?.isLoggedIn;

    return !isLoggedIn && !this.shouldIgnoreLoginSuggestion && this.generatedUrlCount == 3;
  }

  get redirectUrlAfterLogin() {
    return encodeURIComponent(`/url-shortener?url=${this.url}`); 
  }

  constructor(
    private urlsClient: UrlsClient,
    private userService: UserService,
    private route: ActivatedRoute,
    private notificationService: NotificationService
  ) {
    this.route.queryParams.subscribe(params => {
      this.url = params['url'];
    });
  }

  ngOnInit(): void {
    this.userService.getCurrentUser()
      .subscribe(response => this.currentUserResponse = response);
  }

  resetAllProps(): void {
    this.showResult = false;
    this.result = '';
    this.showCopiedNotification = false;
    this.showErrorNotification = false;
    this.errorMessage = '';
    this.notificationService.clearError();
  }

  shorten(): void {

    if (this.shouldSuggestLogin) {
      this.showLoginSuggestionModal = true;
      return;
    }

    this.resetAllProps();
    this.isShortening = true;

    const command = new ShortenUrlCommand({ url: this.url });

    this.urlsClient.shortenUrl(command).subscribe({
      next: (res) => {
        this.result = res;
        this.isShortening = false;
        this.showResult = true;
        this.generatedUrlCount++;
      },
      error: () => {
        this.isShortening = false;
        this.showResult = false;
      }
    });
  }

  onCopyClick(): void {
    navigator.clipboard.writeText(this.result).then(
      () => {
        this.showCopiedNotification = true;

        setTimeout(() => this.showCopiedNotification = false, 3000);
      },
      (err) => console.error('Failed to copy: ', err)
    );
  }

  onCloseResultClick(): void {
    this.showResult = false;
    this.result = '';
  }

  showError(message: string): void {
    this.errorMessage = message;
    this.showErrorNotification = true;
  }

  onCloseErrorClick(): void {
    this.showErrorNotification = false;
    this.errorMessage = '';
  }

  onCloseLoginSuggestionModalClick(): void {
    this.showLoginSuggestionModal = false;
    this.shouldIgnoreLoginSuggestion = true;
  }
}
