export interface UserRepository {
  login(username: string, password: string, rememberMe: boolean): Promise<void>
  refreshToken(): Promise<void>
}
