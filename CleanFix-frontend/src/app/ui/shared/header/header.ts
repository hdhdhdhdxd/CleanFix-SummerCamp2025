import { Component, HostListener, inject, signal } from '@angular/core'
import { Router } from '@angular/router'

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.html',
})
export class Header {
  isScrolled = signal(false)
  router = inject(Router)
  menuOpen = signal(false)

  @HostListener('window:scroll')
  onWindowScroll() {
    this.isScrolled.set(window.scrollY > 50)
  }

  isHome(): boolean {
    return (
      this.router.url === '/' ||
      this.router.url === '/#hero' ||
      this.router.url === '/#services' ||
      this.router.url === '/#footer'
    )
  }

  toggleMenu() {
    this.menuOpen.update((open) => !open)
  }

  closeMenu() {
    this.menuOpen.set(false)
  }
}
