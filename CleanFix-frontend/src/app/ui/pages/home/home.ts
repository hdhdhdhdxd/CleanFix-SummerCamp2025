import { Component, inject, signal } from '@angular/core'
import { of } from 'rxjs'
import { take, switchMap, catchError } from 'rxjs/operators'
import { Hero } from './components/hero/hero'
import { Brands } from './components/brands/brands'
import { ServiceSection } from './components/service-section/service-section'
import { WhyUsSection } from './components/why-us-section/why-us-section'
import { LocationsSection } from './components/locations-section/locations-section'
import { StatsSection } from './components/stats-section/stats-section'
import { UserService } from '@/ui/services/user/user-service'
import { Header } from '@/ui/shared/header/header'
import { User } from '@/core/domain/models/User'

@Component({
  selector: 'app-home',
  imports: [Header, Hero, Brands, ServiceSection, WhyUsSection, LocationsSection, StatsSection],
  templateUrl: './home.html',
})
export class Home {
  private userService = inject(UserService)
  isAdmin = signal(false)
  username = signal<string | undefined>(undefined)

  constructor() {
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
    const hasAdminRole = user?.roles?.includes('Administrator') ?? false
    this.isAdmin.set(hasAdminRole)

    const userUsername = user?.username || undefined
    this.username.set(userUsername)
  }

  /**
   * Resetea el estado del usuario en caso de error
   */
  private resetUserState(): void {
    this.isAdmin.set(false)
    this.username.set(undefined)
  }
}
