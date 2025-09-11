import { inject } from '@angular/core'
import { CanActivateFn, Router } from '@angular/router'
import { UserService } from '../../services/user/user-service'
import { map, catchError, switchMap, take } from 'rxjs/operators'
import { of } from 'rxjs'

export const authGuard: CanActivateFn = () => {
  const userService = inject(UserService)
  const router = inject(Router)

  return userService.isAuthenticated().pipe(
    take(1),
    switchMap((isAuth) => {
      if (isAuth) return of(true)

      return userService.refreshToken().pipe(
        switchMap(() =>
          userService.isAuthenticated().pipe(
            take(1),
            map((nowAuth) => (nowAuth ? true : router.createUrlTree(['/auth/login']))),
          ),
        ),
        catchError(() => of(router.createUrlTree(['/auth/login']))),
      )
    }),
    catchError(() => of(router.createUrlTree(['/auth/login']))),
  )
}
