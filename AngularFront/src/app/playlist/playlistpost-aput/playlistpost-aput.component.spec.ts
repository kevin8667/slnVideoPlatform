import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistpostAputComponent } from './playlistpost-aput.component';

describe('PlaylistpostAputComponent', () => {
  let component: PlaylistpostAputComponent;
  let fixture: ComponentFixture<PlaylistpostAputComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistpostAputComponent]
    });
    fixture = TestBed.createComponent(PlaylistpostAputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
