import { Component, HostListener, signal } from '@angular/core'

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.html',
})
export class Header {
  isScrolled = signal(false)

  @HostListener('window:scroll')
  onWindowScroll() {
    this.isScrolled.set(window.scrollY > 50)
  }
}
