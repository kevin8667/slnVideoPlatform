import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MmainComponent } from './mmain.component';

describe('MmainComponent', () => {
  let component: MmainComponent;
  let fixture: ComponentFixture<MmainComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MmainComponent]
    });
    fixture = TestBed.createComponent(MmainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
