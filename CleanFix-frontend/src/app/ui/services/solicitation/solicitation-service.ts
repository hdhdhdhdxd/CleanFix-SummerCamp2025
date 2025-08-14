import { solicitationService } from '@/core/application/solicitationService'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'
import { Solicitation } from '@/core/domain/models/Solicitation'

@Injectable({
  providedIn: 'root',
})
export class SolicitationService {
  getAll(): Observable<Solicitation[]> {
    return from(solicitationService.getAll())
  }
}
