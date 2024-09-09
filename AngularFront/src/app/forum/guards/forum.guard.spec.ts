import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { forumGuard } from './forum.guard';

describe('forumGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
      TestBed.runInInjectionContext(() => forumGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
