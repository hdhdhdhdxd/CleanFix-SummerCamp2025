import { companyService } from '@/core/application/companyService'
import { incidenceService } from '@/core/application/incidenceService'
import { solicitationService } from '@/core/application/solicitationService'
import { companyApiRepository } from '@/core/infrastructure/api/cleanfix-api/company-repository/companyApiRepository'
import { incidenceApiRepository } from '@/core/infrastructure/api/cleanfix-api/incidence-repository/incidenceApiRepository'
import { solicitationApiRepository } from '@/core/infrastructure/api/cleanfix-api/solicitation-repository/solicitationApiRepository'

export const initializeRepositories = () => {
  solicitationService.init(solicitationApiRepository)
  incidenceService.init(incidenceApiRepository)
  companyService.init(companyApiRepository)
}
