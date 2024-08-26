import { TestBed } from '@angular/core/testing';

import { VideoDBServiceService } from './video-dbservice.service';

describe('VideoDBServiceService', () => {
  let service: VideoDBServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VideoDBServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
