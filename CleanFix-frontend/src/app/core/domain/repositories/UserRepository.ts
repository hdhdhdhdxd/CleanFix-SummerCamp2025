import { User } from '../models/User'

export interface UserRepository {
  login(username: string, password: string, rememberMe: boolean): Promise<void>
  refreshToken(): Promise<void>
  logout(): Promise<void>
  me(): Promise<User>
  isAuthenticated(): Promise<boolean>
}
