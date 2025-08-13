import { companyService } from '@/core/application/companyService'
import { Injectable } from '@angular/core'
import { from } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  getAll() {
    return from(companyService.getAll())
  }
}
