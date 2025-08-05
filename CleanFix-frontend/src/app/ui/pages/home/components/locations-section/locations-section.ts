import { Component, ElementRef, ViewChild } from '@angular/core'
import { ImageCard } from '../image-card/image-card'

@Component({
  selector: 'app-locations-section',
  imports: [ImageCard],
  templateUrl: './locations-section.html',
})
export class LocationsSection {
  @ViewChild('carousel') carousel!: ElementRef

  scrollCarousel(direction: 'left' | 'right') {
    const scrollAmount = 300 // Ajusta según el tamaño de tus cards
    const currentScroll = this.carousel.nativeElement.scrollLeft

    if (direction === 'left') {
      this.carousel.nativeElement.scrollTo({
        left: currentScroll - scrollAmount,
        behavior: 'smooth',
      })
    } else {
      this.carousel.nativeElement.scrollTo({
        left: currentScroll + scrollAmount,
        behavior: 'smooth',
      })
    }
  }
}
