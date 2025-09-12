import { userService } from '@/core/application/userService'
import { User } from '@/core/domain/models/User'
import { Injectable, inject } from '@angular/core'
import { from, Observable, tap } from 'rxjs'
import { AuthStateService } from '../auth-state/auth-state.service'

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private authStateService = inject(AuthStateService)

  login(username: string, password: string, rememberMe: boolean): Observable<void> {
    return from(userService.login(username, password, rememberMe))
  }

  refreshToken(): Observable<void> {
    return from(userService.refreshToken())
  }

  logout(): Observable<void> {
    return from(userService.logout()).pipe(
      tap(() => {
        // Limpiar el estado de autenticación al hacer logout
        this.authStateService.clearUser()
      }),
    )
  }

  me(): Observable<User> {
    return from(userService.me())
  }

  isAuthenticated(): Observable<boolean> {
    return from(userService.isAuthenticated())
  }
}
