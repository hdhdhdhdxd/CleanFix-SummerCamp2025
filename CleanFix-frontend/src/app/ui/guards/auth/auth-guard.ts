import { inject } from '@angular/core'
import { CanActivateFn, Router } from '@angular/router'
import { AuthStateService } from '../../services/auth-state/auth-state.service'
import { of } from 'rxjs'

export const authGuard: CanActivateFn = () => {
  const authState = inject(AuthStateService)
  const router = inject(Router)

  // Si ya está autenticado, permitir acceso
  if (authState.getCurrentIsLoggedIn()) return of(true)

  // Si el refresh ya falló, redirigir al login
  if (authState.hasRefreshFailed && authState.hasRefreshFailed()) {
    return of(router.createUrlTree(['/auth/login']))
  }

  // Si hay un refresh en curso, esperar a que termine y luego decidir
  const refreshPromise = authState.getRefreshInProgress && authState.getRefreshInProgress()
  if (refreshPromise) {
    return new Promise((resolve) => {
      refreshPromise.then(() => {
        resolve(authState.getCurrentIsLoggedIn() ? true : router.createUrlTree(['/auth/login']))
      })
    })
  }

  // Por defecto, redirigir al login (no debería llegar aquí si el flujo global está bien)
  return of(router.createUrlTree(['/auth/login']))
}
