import { companyService } from '@/core/application/companyService'
import { Company } from '@/core/domain/models/Company'
import { PaginationDto } from '@/core/domain/models/PaginationDto'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CompanyService {
  getPaginated(pageNumber: number, pageSize: number): Observable<PaginationDto<Company>> {
    return from(companyService.getPaginated(pageNumber, pageSize))
  }
}
