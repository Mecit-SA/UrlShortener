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

  constructor(private urlsClient: UrlsClient) { }

  shorten(): void {

    this.isShortening = true;

    const command = new ShortenUrlCommand({ url: this.url });

    this.urlsClient.shortenUrl(command).subscribe((res) => {
      this.isShortening = false;
      alert('your url is: ' + res);
    });
  }
}
