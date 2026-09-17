import { Component, Input } from '@angular/core';

@Component({
  selector: 'info-page',
  standalone: false,
  templateUrl: './info-page.component.html',
  styleUrl: './info-page.component.css'
})
export class InfoPageComponent {
  @Input() pageName?: string;

  constructor() {
  }
}