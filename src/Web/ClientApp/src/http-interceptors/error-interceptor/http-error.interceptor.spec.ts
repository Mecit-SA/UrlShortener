import { TestBed, inject } from '@angular/core/testing';

import { HttpErrorInterceptor } from './http-error.interceptor';

describe('HttpErrorInterceptor', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpErrorInterceptor]
    });
  });

  it('should be created', inject([HttpErrorInterceptor], (service: HttpErrorInterceptor) => {
    expect(service).toBeTruthy();
  }));
});
