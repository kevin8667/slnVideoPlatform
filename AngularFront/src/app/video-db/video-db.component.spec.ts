import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoDBComponent } from './video-db.component';

describe('VideoDBComponent', () => {
  let component: VideoDBComponent;
  let fixture: ComponentFixture<VideoDBComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoDBComponent]
    });
    fixture = TestBed.createComponent(VideoDBComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
