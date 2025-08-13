import { CompanyService } from '@/ui/services/company/company-service'
import { AsyncPipe } from '@angular/common'
import { Component, inject } from '@angular/core'

@Component({
  selector: 'app-service-request',
  imports: [AsyncPipe],
  templateUrl: './service-request.html',
})
export class ServiceRequest {
  companyService = inject(CompanyService)
  companies = this.companyService.getAll()
}
