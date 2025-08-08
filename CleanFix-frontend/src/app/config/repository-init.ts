// Inicialización de todos los repositorios en un solo lugar
import { buildingService } from '@/core/application/buildingService'
import { buildingApiRepository } from '@/core/infrastructure/repositories/buildingApiRepository'

export const initializeRepositories = () => {
  buildingService.init(buildingApiRepository)
}
