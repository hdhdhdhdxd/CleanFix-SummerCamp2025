import { companyService } from '@/core/application/companyService'
import { completedTaskService } from '@/core/application/completedTaskService'
import { incidenceService } from '@/core/application/incidenceService'
import { materialService } from '@/core/application/materialService'
import { solicitationService } from '@/core/application/solicitationService'
import { companyApiRepository } from '@/core/infrastructure/api/cleanfix-api/company-repository/companyApiRepository'
import { completedTaskApiRepository } from '@/core/infrastructure/api/cleanfix-api/completedtask-repository/completedTaskApiRepository'
import { incidenceApiRepository } from '@/core/infrastructure/api/cleanfix-api/incidence-repository/incidenceApiRepository'
import { materialApiRepository } from '@/core/infrastructure/api/cleanfix-api/material-repository/materialApiRepository'
import { solicitationApiRepository } from '@/core/infrastructure/api/cleanfix-api/solicitation-repository/solicitationApiRepository'

export const initializeRepositories = () => {
  solicitationService.init(solicitationApiRepository)
  incidenceService.init(incidenceApiRepository)
  companyService.init(companyApiRepository)
  materialService.init(materialApiRepository)
  completedTaskService.init(completedTaskApiRepository)
}
