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

  constructor(private urlsClient: UrlsClient) { }

  shorten(): void {

    this.isShortening = true;
    this.showResult = false;

    const command = new ShortenUrlCommand({ url: this.url });

    this.urlsClient.shortenUrl(command).subscribe((res) => {
      this.result = res;
      this.isShortening = false;
      this.showResult = true;
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
}
