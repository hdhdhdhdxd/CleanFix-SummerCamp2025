import { userService } from '@/core/application/userService'
import { User } from '@/core/domain/models/User'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class UserService {
  login(username: string, password: string, rememberMe: boolean): Observable<void> {
    return from(userService.login(username, password, rememberMe))
  }

  refreshToken(): Observable<void> {
    return from(userService.refreshToken())
  }

  logout(): Observable<void> {
    return from(userService.logout())
  }

  me(): Observable<User> {
    return from(userService.me())
  }
}
