import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForDriverComponent } from './for-driver.component';

describe('ForDriverComponent', () => {
  let component: ForDriverComponent;
  let fixture: ComponentFixture<ForDriverComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ForDriverComponent]
    });
    fixture = TestBed.createComponent(ForDriverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
