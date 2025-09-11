import { UserRepository } from '@/core/domain/repositories/UserRepository'
import { environment } from 'src/environments/environment'
import { HttpClient } from '@angular/common/http'
import { firstValueFrom } from 'rxjs'
import { User } from '@/core/domain/models/User'
import { UserDto } from './UserDto'

export class UserApiRepository implements UserRepository {
  constructor(private http: HttpClient) {}

  async login(email: string, password: string, rememberMe: boolean): Promise<void> {
    await firstValueFrom(
      this.http.post(
        `${environment.baseUrl}auth/login`,
        { email, password, rememberMe },
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      ),
    )
  }

  async refreshToken(): Promise<void> {
    await firstValueFrom(
      this.http.post(
        `${environment.baseUrl}auth/refresh`,
        {},
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      ),
    )
  }

  async logout(): Promise<void> {
    await firstValueFrom(
      this.http.post(
        `${environment.baseUrl}auth/logout`,
        {},
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      ),
    )
  }

  async me(): Promise<User> {
    const reponse = this.http.get<UserDto>(`${environment.baseUrl}auth/me`, {
      withCredentials: true,
      headers: { 'Content-Type': 'application/json' },
    })

    const userDto = await firstValueFrom(reponse)

    return {
      email: userDto.email,
      username: userDto.username,
      roles: userDto.roles,
    }
  }
}
