import { TestBed } from '@angular/core/testing';

import { ServerHttpInterceptor } from './server-http.interceptor';

describe('ServerHttpInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ServerHttpInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: ServerHttpInterceptor = TestBed.inject(ServerHttpInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
