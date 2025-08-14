import { companyService } from '@/core/application/companyService'
import { solicitationService } from '@/core/application/solicitationService'
import { companyApiRepository } from '@/core/infrastructure/repositories/company/companyApiRepository'
import { solicitationApiRepository } from '@/core/infrastructure/repositories/solicitation/solicitationApiRepository'

export const initializeRepositories = () => {
  solicitationService.init(solicitationApiRepository)
  companyService.init(companyApiRepository)
}
