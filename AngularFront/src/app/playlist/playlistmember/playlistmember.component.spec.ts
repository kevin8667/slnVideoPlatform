import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistMemberComponent } from './playlistmember.component';

describe('PlaylistmemberComponent', () => {
  let component: PlaylistMemberComponent;
  let fixture: ComponentFixture<PlaylistMemberComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistMemberComponent]
    });
    fixture = TestBed.createComponent(PlaylistMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
