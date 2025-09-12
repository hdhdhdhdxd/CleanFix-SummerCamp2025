import { Component, HostListener, inject, signal, input } from '@angular/core'
import { Router } from '@angular/router'
import { RouterLink } from '@angular/router'
import { UserService } from '../../services/user/user-service'
import { AuthStateService } from '../../services/auth-state/auth-state.service'

@Component({
  selector: 'app-header',
  imports: [RouterLink],
  templateUrl: './header.html',
})
export class Header {
  isScrolled = signal(false)
  router = inject(Router)
  menuOpen = signal(false)
  menuClosing = signal(false)
  showAdmin = input<boolean>()
  showLogout = input<boolean>()
  userService = inject(UserService)
  authStateService = inject(AuthStateService)

  @HostListener('window:scroll')
  onWindowScroll() {
    this.isScrolled.set(window.scrollY > 50)
  }

  logout() {
    this.userService.logout().subscribe({
      next: () => {
        this.router.navigate(['/auth/login'])
      },
      error: () => {
        this.router.navigate(['/auth/login'])
      },
    })
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
    this.menuClosing.set(true)
    setTimeout(() => {
      this.menuOpen.set(false)
      this.menuClosing.set(false)
    }, 300) // Duración de la animación
  }
}
