import { companyService } from '@/core/application/companyService'
import { Company } from '@/core/domain/models/Company'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  getPaginated(
    pageNumber: number,
    pageSize: number,
    typeIssueId?: number,
  ): Observable<PaginatedData<Company>> {
    return from(companyService.getPaginated(pageNumber, pageSize, typeIssueId))
  }
}
