// Inicialización de todos los repositorios en un solo lugar
import { solicitationService } from '@/core/application/solicitationService'
import { solicitationApiRepository } from '@/core/infrastructure/repositories/solicitationApiRepository'

export const initializeRepositories = () => {
  solicitationService.init(solicitationApiRepository)
}
