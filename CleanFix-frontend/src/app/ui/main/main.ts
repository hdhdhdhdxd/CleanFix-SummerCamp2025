import { CommonModule } from '@angular/common'
import { Component, inject } from '@angular/core'
import { NavigationEnd, Router, RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'
import { Snackbar } from '../shared/snackbar/snackbar'
import { Header } from '../shared/header/header'
import { UserService } from '@/ui/services/user/user-service'
import { AuthStateService } from '@/ui/services/auth-state/auth-state.service'
import { User } from '@/core/domain/models/User'
import { of } from 'rxjs'
import { take, switchMap, catchError } from 'rxjs/operators'

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
  private lastUrl: string | null = null

  // Exponer signals del AuthStateService para usar en el template
  isAdmin = this.authStateService.isAdmin
  isLoggedIn = this.authStateService.isLoggedIn

  constructor() {
    this.router.events.subscribe((event: unknown) => {
      if (event instanceof NavigationEnd) {
        // Oculta el footer en la ruta /service-request
        this.showFooter = !event.urlAfterRedirects.startsWith('/service-request')

        // Si la URL cambió, re-ejecutar la inicialización del estado de usuario
        if (this.lastUrl !== event.urlAfterRedirects) {
          this.lastUrl = event.urlAfterRedirects
          this.initializeUserState()
        }
      }
    })

    this.initializeUserState()
  }

  /**
   * Inicializa el estado del usuario verificando autenticación
   * y obteniendo información del perfil
   */
  private initializeUserState(): void {
    this.userService
      .isAuthenticated()
      .pipe(
        take(1),
        switchMap((isAuthenticated) => this.handleAuthenticationFlow(isAuthenticated)),
      )
      .subscribe({
        next: (user) => this.updateUserState(user),
        error: () => this.resetUserState(),
      })
  }

  /**
   * Maneja el flujo de autenticación basado en el estado actual
   */
  private handleAuthenticationFlow(isAuthenticated: boolean) {
    if (isAuthenticated) {
      return this.getUserProfile()
    }

    // Si no está autenticado, intentar refrescar el token y obtener el perfil
    return this.refreshTokenAndGetProfile()
  }

  /**
   * Obtiene el perfil del usuario autenticado
   */
  private getUserProfile() {
    return this.userService.me().pipe(
      take(1),
      catchError(() => of(null)),
    )
  }

  /**
   * Intenta refrescar el token y obtener el perfil del usuario
   */
  private refreshTokenAndGetProfile() {
    return this.userService.refreshToken().pipe(
      switchMap(() => this.getUserProfile()),
      catchError(() => of(null)),
    )
  }

  /**
   * Actualiza el estado del usuario con la información obtenida
   */
  private updateUserState(user: User | null): void {
    this.authStateService.setUser(user)
  }

  /**
   * Resetea el estado del usuario en caso de error
   */
  private resetUserState(): void {
    this.authStateService.clearUser()
  }
}
