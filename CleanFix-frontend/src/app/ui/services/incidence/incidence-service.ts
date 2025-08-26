import { incidenceService } from '@/core/application/incidenceService'
import { Incidence } from '@/core/domain/models/Incedence'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class IncidenceService {
  getPaginated(pageNumber: number, pageSize: number): Observable<PaginatedData<Incidence>> {
    return from(incidenceService.getPaginated(pageNumber, pageSize))
  }
}
