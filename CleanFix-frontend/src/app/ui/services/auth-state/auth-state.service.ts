import { Injectable, signal, computed } from '@angular/core'
import { BehaviorSubject } from 'rxjs'
import { User } from '@/core/domain/models/User'

export interface AuthState {
  user: User | null
  isLoading: boolean
}

@Injectable({
  providedIn: 'root',
})
export class AuthStateService {
  private _authState = signal<AuthState>({
    user: null,
    isLoading: true,
  })

  // Signals derivadas para uso en componentes
  user = computed(() => this._authState().user)
  isLoggedIn = computed(() => !!this._authState().user)
  isAdmin = computed(() => this._authState().user?.roles?.includes('Administrator') ?? false)
  isLoading = computed(() => this._authState().isLoading)

  // Observable para compatibilidad con código existente
  private _authState$ = new BehaviorSubject<AuthState>({
    user: null,
    isLoading: true,
  })

  authState$ = this._authState$.asObservable()

  /**
   * Actualiza el estado de autenticación con nueva información del usuario
   */
  setUser(user: User | null): void {
    const newState: AuthState = {
      user,
      isLoading: false,
    }
    this._authState.set(newState)
    this._authState$.next(newState)
  }

  /**
   * Marca el estado como cargando
   */
  setLoading(loading: boolean): void {
    const currentState = this._authState()
    const newState: AuthState = {
      ...currentState,
      isLoading: loading,
    }
    this._authState.set(newState)
    this._authState$.next(newState)
  }

  /**
   * Resetea el estado de autenticación (logout)
   */
  clearUser(): void {
    const newState: AuthState = {
      user: null,
      isLoading: false,
    }
    this._authState.set(newState)
    this._authState$.next(newState)
  }

  /**
   * Obtiene el estado actual de autenticación
   */
  getCurrentUser(): User | null {
    return this._authState().user
  }

  /**
   * Verifica si el usuario actual está autenticado
   */
  getCurrentIsLoggedIn(): boolean {
    return this.isLoggedIn()
  }

  /**
   * Verifica si el usuario actual es administrador
   */
  getCurrentIsAdmin(): boolean {
    return this.isAdmin()
  }
}
