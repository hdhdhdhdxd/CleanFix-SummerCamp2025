import { Component, input } from '@angular/core'

@Component({
  selector: 'app-service-card',
  imports: [],
  templateUrl: './service-card.html',
})
export class ServiceCard {
  title = input<string>('Título')
  description = input<string>('Descripción')
  svgSource = input<string>('/assets/svg/drill.svg')
}
