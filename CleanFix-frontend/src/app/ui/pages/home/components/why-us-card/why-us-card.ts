import { Component, input } from '@angular/core'

@Component({
  selector: 'app-why-us-card',
  imports: [],
  templateUrl: './why-us-card.html',
})
export class WhyUsCard {
  title = input<string>('Title')
  description = input<string>('Description')
  svgSource = input<string>('SVG Source')
}
