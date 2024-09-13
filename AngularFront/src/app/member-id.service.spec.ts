import { TestBed } from '@angular/core/testing';

import { MemberIDService } from './member-id.service';

describe('MemberIDService', () => {
  let service: MemberIDService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MemberIDService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
