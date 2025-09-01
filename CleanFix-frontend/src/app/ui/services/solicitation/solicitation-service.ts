import { solicitationService } from '@/core/application/solicitationService'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class SolicitationService {
  getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Observable<PaginatedData<SolicitationBrief>> {
    return from(solicitationService.getPaginated(pageNumber, pageSize, filterString))
  }

  getById(id: number): Observable<Solicitation> {
    return from(solicitationService.getById(id))
  }
}
