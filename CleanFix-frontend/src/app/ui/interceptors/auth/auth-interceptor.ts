import { UserService } from '@/ui/services/user/user-service'
import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http'
import { inject } from '@angular/core'
import { Router } from '@angular/router'
import { catchError, of, switchMap } from 'rxjs'

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const userService = inject(UserService)
  const router = inject(Router)

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return userService.refreshToken().pipe(
          switchMap(() => next(req)),
          catchError((err) => {
            console.error('Token refresh failed', err)
            router.navigate(['/auth/login'])
            return of()
          }),
        )
      }
      return of()
    }),
  )
}
