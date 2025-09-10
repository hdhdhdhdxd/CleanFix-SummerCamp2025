import { UserRepository } from '@/core/domain/repositories/UserRepository'
import { environment } from 'src/environments/environment'

const login = async (email: string, password: string, rememberMe: boolean) => {
  const response = await fetch(`${environment.baseUrl}users/login`, {
    method: 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password, rememberMe }),
  })

  if (!response.ok) {
    throw new Error('Error al iniciar sesión')
  }
}

const refreshToken = async () => {
  const response = await fetch(`${environment.baseUrl}users/refresh-token`, {
    method: 'POST',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
    },
  })

  if (!response.ok) {
    throw new Error('Error al refrescar el token')
  }
}

export const userApiRepository: UserRepository = {
  login: login,
  refreshToken: refreshToken,
}
