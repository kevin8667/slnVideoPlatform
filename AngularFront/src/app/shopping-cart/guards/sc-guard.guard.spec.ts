import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { scGuardGuard } from './sc-guard.guard';

describe('scGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => scGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
