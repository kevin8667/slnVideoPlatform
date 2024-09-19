import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistitemComponent } from './playlistitem.component';

describe('PlaylistitemComponent', () => {
  let component: PlaylistitemComponent;
  let fixture: ComponentFixture<PlaylistitemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistitemComponent]
    });
    fixture = TestBed.createComponent(PlaylistitemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
