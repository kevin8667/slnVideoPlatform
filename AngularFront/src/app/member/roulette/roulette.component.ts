import { Component } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-roulette',
  templateUrl: './roulette.component.html',
  styleUrls: ['./roulette.component.css'],
  animations: [
    trigger('spin', [
      state('spinning', style({ transform: 'rotate(360deg)' })),
      transition('* => spinning', [
        animate('2s ease-out')
      ])
    ])
  ]
})
export class RouletteComponent {
  segments = [
    { label: 'Prize 1' },
    { label: 'Prize 2' },
    { label: 'Prize 3' },
    { label: 'Prize 4' }
  ];

  spinState = '';

  spinWheel() {
    this.spinState = 'spinning';

    setTimeout(() => {
      this.spinState = '';
    }, 2000);
  }
}
