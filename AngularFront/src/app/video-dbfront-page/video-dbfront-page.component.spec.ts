import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoDBFrontPageComponent } from './video-dbfront-page.component';

describe('VideoDBFrontPageComponent', () => {
  let component: VideoDBFrontPageComponent;
  let fixture: ComponentFixture<VideoDBFrontPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoDBFrontPageComponent]
    });
    fixture = TestBed.createComponent(VideoDBFrontPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
