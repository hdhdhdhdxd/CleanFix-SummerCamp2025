import { completedTaskService } from '@/core/application/completedTaskService'
import { Injectable } from '@angular/core'
import { Observable, from } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class CompletedTaskService {
  create(
    solicitationId: number,
    incidenceId: number,
    companyId: number,
    isSolicitation: boolean,
    MaterialIds: number[],
  ): Observable<void> {
    return from(
      completedTaskService.create(
        solicitationId,
        incidenceId,
        companyId,
        isSolicitation,
        MaterialIds,
      ),
    )
  }
}
