import { solicitationService } from '@/core/application/solicitationService'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class SolicitationService {
  getPaginated(pageNumber: number, pageSize: number): Observable<PaginatedData<Solicitation>> {
    return from(solicitationService.getPaginated(pageNumber, pageSize))
  }
}
