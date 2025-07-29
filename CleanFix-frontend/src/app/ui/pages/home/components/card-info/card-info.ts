import { Component, input } from '@angular/core'

@Component({
  selector: 'app-card-info',
  imports: [],
  templateUrl: './card-info.html',
})
export class CardInfo {
  svgPath = input<string>('')
  title = input<string>('')
  value = input<number>(0)
}
