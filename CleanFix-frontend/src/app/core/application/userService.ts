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

export const userService = {
  init,
  login,
  refreshToken,
}
