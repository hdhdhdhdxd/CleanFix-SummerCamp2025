import { CommonModule } from '@angular/common'
import { Component, input } from '@angular/core'

@Component({
  selector: 'app-image-card',
  imports: [CommonModule],
  templateUrl: './image-card.html',
})
export class ImageCard {
  title = input<string>('Pais')
  imageUrl = input<string>('/images/country1.webp')
}
