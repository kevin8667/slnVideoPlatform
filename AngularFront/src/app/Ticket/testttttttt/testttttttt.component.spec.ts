import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TesttttttttComponent } from './testttttttt.component';

describe('TesttttttttComponent', () => {
  let component: TesttttttttComponent;
  let fixture: ComponentFixture<TesttttttttComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TesttttttttComponent]
    });
    fixture = TestBed.createComponent(TesttttttttComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
