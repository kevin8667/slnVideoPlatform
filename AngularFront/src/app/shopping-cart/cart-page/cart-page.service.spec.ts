import { TestBed } from '@angular/core/testing';

import { CartPageService } from './cart-page.service';

describe('CartPageService', () => {
  let service: CartPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CartPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
