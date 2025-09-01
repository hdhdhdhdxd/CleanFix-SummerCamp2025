import { incidenceService } from '@/core/application/incidenceService'
import { Incidence } from '@/core/domain/models/Incidence'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class IncidenceService {
  getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Observable<PaginatedData<IncidenceBrief>> {
    return from(incidenceService.getPaginated(pageNumber, pageSize, filterString))
  }

  getById(id: number): Observable<Incidence> {
    return from(incidenceService.getById(id))
  }
}
