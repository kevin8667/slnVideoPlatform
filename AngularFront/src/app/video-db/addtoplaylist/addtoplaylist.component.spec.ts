import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddtoplaylistComponent } from './addtoplaylist.component';

describe('AddtoplaylistComponent', () => {
  let component: AddtoplaylistComponent;
  let fixture: ComponentFixture<AddtoplaylistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddtoplaylistComponent]
    });
    fixture = TestBed.createComponent(AddtoplaylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
