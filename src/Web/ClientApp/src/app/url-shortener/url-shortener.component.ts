import { Component } from '@angular/core';
import { ShortenUrlCommand, UrlsClient } from '../web-api-client';

@Component({
  selector: 'app-url-shortener',
  templateUrl: './url-shortener.component.html',
  styleUrl: './url-shortener.component.css'
})
export class UrlShortenerComponent {

  url: string;
  isShortening = false;
  showResult = false;
  result: string;
  showCopiedNotification = false;
  showErrorNotification = false;
  errorMessage: string;

  constructor(private urlsClient: UrlsClient) { }

  resetAllProps(): void {
    this.showResult = false;
    this.result = '';
    this.showCopiedNotification = false;
    this.showErrorNotification = false;
    this.errorMessage = '';
  }

  shorten(): void {
    this.resetAllProps();
    this.isShortening = true;

    const command = new ShortenUrlCommand({ url: this.url });

    this.urlsClient.shortenUrl(command).subscribe({
      next: (res) => {
        this.result = res;
        this.isShortening = false;
        this.showResult = true;
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

}
