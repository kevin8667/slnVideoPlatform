import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistmemberComponent } from './playlistmember.component';

describe('PlaylistmemberComponent', () => {
  let component: PlaylistmemberComponent;
  let fixture: ComponentFixture<PlaylistmemberComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistmemberComponent]
    });
    fixture = TestBed.createComponent(PlaylistmemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
