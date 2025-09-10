import { companyService } from '@/core/application/companyService'
import { completedTaskService } from '@/core/application/completedTaskService'
import { incidenceService } from '@/core/application/incidenceService'
import { materialService } from '@/core/application/materialService'
import { solicitationService } from '@/core/application/solicitationService'
import { userService } from '@/core/application/userService'
import { CompanyApiRepository } from '@/core/infrastructure/api/cleanfix-api/company-repository/companyApiRepository'
import { CompletedTaskApiRepository } from '@/core/infrastructure/api/cleanfix-api/completedtask-repository/completedTaskApiRepository'
import { IncidenceApiRepository } from '@/core/infrastructure/api/cleanfix-api/incidence-repository/incidenceApiRepository'
import { MaterialApiRepository } from '@/core/infrastructure/api/cleanfix-api/material-repository/materialApiRepository'
import { SolicitationApiRepository } from '@/core/infrastructure/api/cleanfix-api/solicitation-repository/solicitationApiRepository'
import { UserApiRepository } from '@/core/infrastructure/api/cleanfix-api/user-repository/userApiRepository'
import { HttpClient } from '@angular/common/http'

export const initializeRepositories = (http: HttpClient) => {
  solicitationService.init(new SolicitationApiRepository(http))
  incidenceService.init(new IncidenceApiRepository(http))
  companyService.init(new CompanyApiRepository(http))
  materialService.init(new MaterialApiRepository(http))
  completedTaskService.init(new CompletedTaskApiRepository(http))
  userService.init(new UserApiRepository(http))
}
