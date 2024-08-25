import { Component,AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-video-dbfront-page',
  templateUrl: './video-dbfront-page.component.html',
  styleUrls: ['./video-dbfront-page.component.css']
})
export class VideoDBFrontPageComponent implements AfterViewInit{

  ngAfterViewInit(): void {
    $('.owl-carousel').owlCarousel({
      loop: true,
      margin: 10,
      nav: true,
      responsive: {
        0: {
          items: 1
        },
        600: {
          items: 3
        },
        1000: {
          items: 5
        }
      }
    });
  }

}
