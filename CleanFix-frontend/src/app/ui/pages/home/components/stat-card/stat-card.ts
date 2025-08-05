import { Component, input } from '@angular/core'

@Component({
  selector: 'app-stat-card',
  imports: [],
  templateUrl: './stat-card.html',
})
export class StatCard {
  svgPath = input<string>('')
  title = input<string>('')
  value = input<number>(0)
}
