import { inject } from '@angular/core'
import { CanActivateFn } from '@angular/router'
import { UserService } from '../../services/user/user-service'
import { map, catchError } from 'rxjs/operators'
import { of } from 'rxjs'

export const adminGuard: CanActivateFn = () => {
  const userService = inject(UserService)

  return userService.me().pipe(
    map((user) => !!user && user.roles?.includes('Administrator')),
    catchError(() => of(false)),
  )
}
