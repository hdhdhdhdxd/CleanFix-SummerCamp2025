// Inicialización de todos los repositorios en un solo lugar
import { companyService } from '@/core/application/companyService'
import { solicitationService } from '@/core/application/solicitationService'
import { companyApiRepository } from '@/core/infrastructure/repositories/companyApiRepository'
import { solicitationApiRepository } from '@/core/infrastructure/repositories/solicitationApiRepository'

export const initializeRepositories = () => {
  solicitationService.init(solicitationApiRepository)
  companyService.init(companyApiRepository)
}
