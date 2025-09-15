import { inject } from '@angular/core'
import { CanActivateFn, Router } from '@angular/router'
import { AuthStateService } from '../../services/auth-state/auth-state.service'
import { of } from 'rxjs'

export const guestGuard: CanActivateFn = () => {
  const authState = inject(AuthStateService)
  const router = inject(Router)

  // Si ya está autenticado, redirigir a home
  if (authState.getCurrentIsLoggedIn()) return of(router.createUrlTree(['/']))

  // Si el refresh ya falló, permitir acceso como invitado
  if (authState.hasRefreshFailed && authState.hasRefreshFailed()) {
    return of(true)
  }

  // Si hay un refresh en curso, esperar a que termine y luego decidir
  const refreshPromise = authState.getRefreshInProgress && authState.getRefreshInProgress()
  if (refreshPromise) {
    return new Promise((resolve) => {
      refreshPromise.then(() => {
        resolve(authState.getCurrentIsLoggedIn() ? router.createUrlTree(['/']) : true)
      })
    })
  }

  // Por defecto, permitir acceso como invitado
  return of(true)
}
