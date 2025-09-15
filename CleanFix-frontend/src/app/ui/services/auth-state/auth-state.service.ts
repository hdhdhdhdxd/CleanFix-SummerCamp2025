import { Injectable, signal, computed, inject } from '@angular/core'
import { BehaviorSubject, of } from 'rxjs'
import { switchMap, catchError } from 'rxjs/operators'
import { User } from '@/core/domain/models/User'
import { UserService } from '@/ui/services/user/user-service'

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

  private refreshFailed = false
  private refreshInProgress: Promise<unknown> | null = null

  user = computed(() => this._authState().user)
  isLoggedIn = computed(() => !!this._authState().user)
  isAdmin = computed(() => this._authState().user?.roles?.includes('Administrator') ?? false)
  isLoading = computed(() => this._authState().isLoading)

  private _authState$ = new BehaviorSubject<AuthState>({
    user: null,
    isLoading: true,
  })

  authState$ = this._authState$.asObservable()

  /**
   * Inicializa el estado de autenticación global (para AppInitializer)
   */
  initAuthState(): Promise<void> {
    if (this.getCurrentIsLoggedIn()) return Promise.resolve()
    if (this.hasRefreshFailed()) return Promise.resolve()
    const refreshPromise = this.getRefreshInProgress()
    if (refreshPromise) return refreshPromise as Promise<void>
    const userService = inject(UserService)
    const refresh$ = userService.refreshToken().pipe(
      switchMap(() => userService.me()),
      catchError(() => of(null)),
    )
    const promise = new Promise<void>((resolve) => {
      refresh$.subscribe({
        next: (user) => {
          if (user && typeof user === 'object' && 'username' in user) {
            this.setUser(user)
          } else {
            this.setUser(null)
          }
          resolve()
        },
        error: () => {
          this.setUser(null)
          this.setRefreshFailed()
          resolve()
        },
        complete: () => {
          this.clearRefreshInProgress()
        },
      })
    })
    this.setRefreshInProgress(promise)
    return promise
  }

  setUser(user: User | null): void {
    const newState: AuthState = {
      user,
      isLoading: false,
    }
    this._authState.set(newState)
    this._authState$.next(newState)
  }

  setLoading(loading: boolean): void {
    const currentState = this._authState()
    const newState: AuthState = {
      ...currentState,
      isLoading: loading,
    }
    this._authState.set(newState)
    this._authState$.next(newState)
  }

  clearUser(): void {
    const newState: AuthState = {
      user: null,
      isLoading: false,
    }
    this._authState.set(newState)
    this._authState$.next(newState)
    this.refreshFailed = false
    this.refreshInProgress = null
  }

  setRefreshFailed(): void {
    this.refreshFailed = true
  }

  hasRefreshFailed(): boolean {
    return this.refreshFailed
  }

  setRefreshInProgress(promise: Promise<unknown>): void {
    this.refreshInProgress = promise
  }
  getRefreshInProgress(): Promise<unknown> | null {
    return this.refreshInProgress
  }
  clearRefreshInProgress(): void {
    this.refreshInProgress = null
  }

  getCurrentUser(): User | null {
    return this._authState().user
  }

  getCurrentIsLoggedIn(): boolean {
    return this.isLoggedIn()
  }

  getCurrentIsAdmin(): boolean {
    return this.isAdmin()
  }
}
