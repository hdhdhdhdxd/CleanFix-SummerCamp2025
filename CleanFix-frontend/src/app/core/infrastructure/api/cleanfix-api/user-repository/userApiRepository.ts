import { UserRepository } from '@/core/domain/repositories/UserRepository'
import { environment } from 'src/environments/environment'
import { HttpClient } from '@angular/common/http'
import { firstValueFrom } from 'rxjs'

export class UserApiRepository implements UserRepository {
  constructor(private http: HttpClient) {}

  async login(email: string, password: string, rememberMe: boolean): Promise<void> {
    await firstValueFrom(
      this.http.post(
        `${environment.baseUrl}users/login`,
        { email, password, rememberMe },
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      ),
    )
  }

  async refreshToken(): Promise<void> {
    await firstValueFrom(
      this.http.post(
        `${environment.baseUrl}users/refresh`,
        {},
        { withCredentials: true, headers: { 'Content-Type': 'application/json' } },
      ),
    )
  }
}
