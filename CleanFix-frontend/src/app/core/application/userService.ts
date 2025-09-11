import { UserRepository } from '../domain/repositories/UserRepository'

let userRepository: UserRepository

const init = (repository: UserRepository) => {
  userRepository = repository
}

const login = async (username: string, password: string, rememberMe: boolean) => {
  return userRepository.login(username, password, rememberMe)
}

const refreshToken = async () => {
  return userRepository.refreshToken()
}

const logout = async () => {
  return userRepository.logout()
}

const me = async () => {
  return userRepository.me()
}

const isAuthenticated = async () => {
  return userRepository.isAuthenticated()
}

export const userService = {
  init,
  login,
  refreshToken,
  isAuthenticated,
  logout,
  me,
}
