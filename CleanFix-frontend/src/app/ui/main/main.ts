import { CommonModule } from '@angular/common'
import { Component, inject } from '@angular/core'
import { NavigationEnd, Router, RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'
import { Snackbar } from '../shared/snackbar/snackbar'
import { Header } from '../shared/header/header'
import { UserService } from '@/ui/services/user/user-service'
import { AuthStateService } from '@/ui/services/auth-state/auth-state.service'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Header, Snackbar, CommonModule],
  templateUrl: './main.html',
})
export class Main {
  showFooter = true

  private router = inject(Router)
  private userService = inject(UserService)
  private authStateService = inject(AuthStateService)

  // Exponer signals del AuthStateService para usar en el template
  isAdmin = this.authStateService.isAdmin
  isLoggedIn = this.authStateService.isLoggedIn

  constructor() {
    this.router.events.subscribe((event: unknown) => {
      if (event instanceof NavigationEnd) {
        // Oculta el footer en la ruta /service-request
        this.showFooter = !event.urlAfterRedirects.startsWith('/service-request')
      }
    })
    // Ya no es necesario inicializar el estado de autenticación aquí, se hace en AppInitializer
  }
}
