import { CompanyService } from '@/ui/services/company/company-service'
import { AsyncPipe, NgFor, NgIf } from '@angular/common'
import { Component, inject } from '@angular/core'

@Component({
  selector: 'app-service-request',
  imports: [AsyncPipe, NgFor, NgIf],
  templateUrl: './service-request.html',
})
export class ServiceRequest {
  companyService = inject(CompanyService)
  pagination = this.companyService.getPaginated(1, 10)
}
