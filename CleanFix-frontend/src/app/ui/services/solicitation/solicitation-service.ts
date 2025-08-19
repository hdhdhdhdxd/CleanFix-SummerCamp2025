import { solicitationService } from '@/core/application/solicitationService'
import { PaginationDto } from '@/core/domain/models/PaginationDto'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class SolicitationService {
  getAll(): Observable<PaginationDto<Solicitation>> {
    return from(solicitationService.getAll())
  }
}
