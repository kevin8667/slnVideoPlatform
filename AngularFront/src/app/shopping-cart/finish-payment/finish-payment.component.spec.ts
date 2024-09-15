import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishPaymentComponent } from './finish-payment.component';

describe('FinishPaymentComponent', () => {
  let component: FinishPaymentComponent;
  let fixture: ComponentFixture<FinishPaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FinishPaymentComponent]
    });
    fixture = TestBed.createComponent(FinishPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
