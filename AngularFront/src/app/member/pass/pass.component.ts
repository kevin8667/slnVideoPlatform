// import { Component, AfterViewInit } from '@angular/core';
// import anime from 'animejs/lib/anime.es.js';

// @Component({
//   selector: 'app-pass',
//   templateUrl: './pass.component.html',
//   styleUrls: ['./pass.component.css']
  
// })
// export class PassComponent implements AfterViewInit {
//   firstPass = true;
//   name = '';

//   ngAfterViewInit() {
//     this.setupAnimationEvents();
//   }

//   setupAnimationEvents() {
//     const firstLine = document.getElementById('firstLine');
//     const secondLine = document.getElementById('secondLine');
//     const entryBox = document.getElementById('entryBox');
//     const navBottom = document.getElementById('navBottom');
//     const ready = document.getElementById('ready');
//     const rocketShip = document.getElementById('rocketShip');
//     const lineDrawing = document.querySelector('#lineDrawing .lines path');

//     if (firstLine) {
//       firstLine.addEventListener('animationend', () => {
//         if (this.firstPass) {
//           secondLine?.classList.add('animated', 'fadeInUp');
//           secondLine!.style.visibility = 'visible';
//           document.querySelector('hr')?.classList.add('animated', 'fadeInLeft');
//           document.querySelector('hr')!.style.visibility = 'visible';
//           this.firstPass = false;
//         }
//       });
//     }

//     if (secondLine) {
//       secondLine.addEventListener('animationend', () => {
//         document.body.classList.add('movedBackground');
//       });
//     }

//     document.body.addEventListener('transitionend', () => {
//       entryBox!.style.visibility = 'visible';
//       entryBox?.classList.add('animated', 'fadeIn');
//       navBottom!.style.visibility = 'visible';
//       navBottom?.classList.add('animated', 'fadeIn');
//     });
//   }

//   onInputFocusIn() {
//     const firstLine = document.getElementById('firstLine');
//     const secondLine = document.getElementById('secondLine');
//     const ready = document.getElementById('ready');
//     const rocketShip = document.getElementById('rocketShip');

//     firstLine?.classList.add('fadeOutRight');
//     secondLine?.classList.add('fadeOutLeft');
    
//     ready!.style.visibility = 'visible';
//     ready?.classList.add('fadeIn');
    
//     rocketShip!.style.visibility = 'visible';

//     anime({
//       targets: '#lineDrawing .lines path',
//       strokeDashoffset: [anime.setDashoffset, 0],
//       easing: 'easeInOutSine',
//       duration: 1500,
//       delay: (el:any, i:any) => i * 250,
//       direction: 'normal'
//     });
//   }

//   onInputFocusOut() {
//     const firstLine = document.getElementById('firstLine');
//     const secondLine = document.getElementById('secondLine');
//     const ready = document.getElementById('ready');

//     firstLine?.classList.add('fadeInRight');
//     firstLine?.classList.remove('fadeOutRight', 'fadeInDown');
    
//     secondLine?.classList.add('fadeInLeft');
//     secondLine?.classList.remove('fadeOutLeft', 'fadeInUp');

//     ready?.classList.remove('fadeIn');
//     ready?.classList.add('fadeOut');

//     anime({
//       targets: '#lineDrawing .lines path',
//       strokeDashoffset: [anime.setDashoffset, 0],
//       easing: 'easeInOutSine',
//       duration: 1500,
//       delay: (el:any, i:any) => i * 250,
//       direction: 'reverse'
//     });
//   }

//   moveContent() {
//     this.name = this.getName();
    
//     const content = document.getElementById("content");
    
//     if (content) {
//       content.animate(
//         [{ marginTop: '0px' }, { marginTop: '-800px' }],
//         { duration: 1500, fill: "forwards" }
//       ).onfinish = () => {
//         content.innerHTML = '';
//         content.style.marginTop = "0";
//         content.innerHTML = `<h1 id='afterAnimationHeader' class='animated fadeIn'>${this.name}, this is your story.</h1>`;
//       };
      
//       // 在動畫結束後重置內容
//       setTimeout(() => { 
//         content.style.marginTop = "0"; 
//         content.innerHTML = ""; 
//       }, 1500);
      
//     }
//   }

//   getName(): string {
//     return (document.getElementById("mainInput") as HTMLInputElement).value;
//   }

//   onKeyUp(event: KeyboardEvent) {
//     if (event.key === "Enter") {
//       this.moveContent();
//     }
// }}