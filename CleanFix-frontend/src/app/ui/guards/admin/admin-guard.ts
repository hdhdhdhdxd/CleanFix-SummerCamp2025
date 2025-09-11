import { inject } from '@angular/core'
import { CanActivateFn, Router } from '@angular/router'
import { UserService } from '../../services/user/user-service'
import { map, catchError, switchMap, take } from 'rxjs/operators'
import { of } from 'rxjs'

export const adminGuard: CanActivateFn = () => {
  const userService = inject(UserService)
  const router = inject(Router)

  return userService.me().pipe(
    take(1),
    switchMap((user) => {
      if (user)
        return of(
          user && (user.roles?.includes('Administrator') ?? false)
            ? true
            : router.createUrlTree(['/']),
        )

      return userService.refreshToken().pipe(
        switchMap(() =>
          userService.me().pipe(
            take(1),
            map((u) =>
              u && u.roles?.includes('Administrator') ? true : router.createUrlTree(['/']),
            ),
          ),
        ),
        catchError(() => of(router.createUrlTree(['/auth/login']))),
      )
    }),
    catchError(() => of(router.createUrlTree(['/auth/login']))),
  )
}
