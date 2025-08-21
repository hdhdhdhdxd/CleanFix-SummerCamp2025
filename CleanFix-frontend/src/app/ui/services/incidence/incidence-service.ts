import { incidenceService } from '@/core/application/incidenceService'
import { Incidence } from '@/core/domain/models/Incedence'
import { PaginationDto } from '@/core/domain/models/PaginationDto'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class IncidenceService {
  getAll(pageNumber: number, pageSize: number): Observable<PaginationDto<Incidence>> {
    return from(incidenceService.getAll(pageNumber, pageSize))
  }
}
