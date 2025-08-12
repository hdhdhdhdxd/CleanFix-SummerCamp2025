import { solicitationService } from '@/core/application/solicitationService'
import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class SolicitationService {
  getAll() {
    return solicitationService.getAll()
  }
}
