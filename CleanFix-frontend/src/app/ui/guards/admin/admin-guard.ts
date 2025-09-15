import { inject } from '@angular/core'
import { CanActivateFn, Router } from '@angular/router'
import { AuthStateService } from '../../services/auth-state/auth-state.service'
import { of } from 'rxjs'

export const adminGuard: CanActivateFn = () => {
  const authState = inject(AuthStateService)
  const router = inject(Router)

  // Si ya está autenticado y es admin, permitir acceso
  if (authState.getCurrentIsAdmin && authState.getCurrentIsAdmin()) return of(true)

  // Si el refresh ya falló, redirigir al login
  if (authState.hasRefreshFailed && authState.hasRefreshFailed()) {
    return of(router.createUrlTree(['/auth/login']))
  }

  // Si hay un refresh en curso, esperar a que termine y luego decidir
  const refreshPromise = authState.getRefreshInProgress && authState.getRefreshInProgress()
  if (refreshPromise) {
    return new Promise((resolve) => {
      refreshPromise.then(() => {
        resolve(
          authState.getCurrentIsAdmin && authState.getCurrentIsAdmin()
            ? true
            : router.createUrlTree(['/']),
        )
      })
    })
  }

  // Por defecto, redirigir a home
  return of(router.createUrlTree(['/']))
}
