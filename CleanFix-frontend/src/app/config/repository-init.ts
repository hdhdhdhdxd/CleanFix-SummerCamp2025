import { companyService } from '@/core/application/companyService'
import { incidenceService } from '@/core/application/incidenceService'
import { solicitationService } from '@/core/application/solicitationService'
import { companyApiRepository } from '@/core/infrastructure/repositories/company/companyApiRepository'
import { incidenceApiRepository } from '@/core/infrastructure/repositories/incidence/incidenceApiRepository'
import { solicitationApiRepository } from '@/core/infrastructure/repositories/solicitation/solicitationApiRepository'

export const initializeRepositories = () => {
  solicitationService.init(solicitationApiRepository)
  incidenceService.init(incidenceApiRepository)
  companyService.init(companyApiRepository)
}
