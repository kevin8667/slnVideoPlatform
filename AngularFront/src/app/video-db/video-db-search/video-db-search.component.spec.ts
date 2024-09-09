import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoDbSearchComponent } from './video-db-search.component';

describe('VideoDbSearchComponent', () => {
  let component: VideoDbSearchComponent;
  let fixture: ComponentFixture<VideoDbSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoDbSearchComponent]
    });
    fixture = TestBed.createComponent(VideoDbSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
