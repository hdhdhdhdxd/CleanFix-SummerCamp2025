import { inject, Injectable } from '@angular/core'
import { UserService } from '../user/user-service'
import { BehaviorSubject, catchError, map, Observable, of, tap } from 'rxjs'
import { User } from '@/core/domain/models/User'

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private user$ = new BehaviorSubject<User | null>(null)

  userService = inject(UserService)

  loadUser() {
    return this.userService.me().pipe(
      tap((user) => this.user$.next(user)),
      catchError(() => {
        this.user$.next(null)
        return of(null)
      }),
    )
  }

  getUser(): Observable<User | null> {
    return this.user$.asObservable()
  }

  isAdmin(): Observable<boolean> {
    return this.getUser().pipe(map((user) => !!user && user.roles?.includes('Administrator')))
  }
}
