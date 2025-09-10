import { companyService } from '@/core/application/companyService'
import { completedTaskService } from '@/core/application/completedTaskService'
import { incidenceService } from '@/core/application/incidenceService'
import { materialService } from '@/core/application/materialService'
import { solicitationService } from '@/core/application/solicitationService'
import { userService } from '@/core/application/userService'
import { companyApiRepository } from '@/core/infrastructure/api/cleanfix-api/company-repository/companyApiRepository'
import { completedTaskApiRepository } from '@/core/infrastructure/api/cleanfix-api/completedtask-repository/completedTaskApiRepository'
import { incidenceApiRepository } from '@/core/infrastructure/api/cleanfix-api/incidence-repository/incidenceApiRepository'
import { materialApiRepository } from '@/core/infrastructure/api/cleanfix-api/material-repository/materialApiRepository'
import { SolicitationApiRepository } from '@/core/infrastructure/api/cleanfix-api/solicitation-repository/solicitationApiRepository'
import { userApiRepository } from '@/core/infrastructure/api/cleanfix-api/user-repository/userApiRepository'
import { HttpClient } from '@angular/common/http'

export const initializeRepositories = (http: HttpClient) => {
  solicitationService.init(new SolicitationApiRepository(http))
  incidenceService.init(incidenceApiRepository)
  companyService.init(companyApiRepository)
  materialService.init(materialApiRepository)
  completedTaskService.init(completedTaskApiRepository)
  userService.init(userApiRepository)
}
