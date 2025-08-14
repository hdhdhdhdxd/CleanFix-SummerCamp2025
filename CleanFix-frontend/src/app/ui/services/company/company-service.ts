import { companyService } from '@/core/application/companyService'
import { Company } from '@/core/domain/models/Company'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  getAll(): Observable<Company[]> {
    return from(companyService.getAll())
  }
}
