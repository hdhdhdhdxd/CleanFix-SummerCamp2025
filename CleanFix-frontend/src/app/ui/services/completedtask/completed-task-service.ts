import { completedTaskService } from '@/core/application/completedTaskService'
import { CompletedTask } from '@/core/domain/models/CompletedTask'
import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
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

  getPaginated(
    pageNumber: number,
    pageSize: number,
  ): Observable<PaginatedData<CompletedTaskBrief>> {
    return from(completedTaskService.getPaginated(pageNumber, pageSize))
  }

  getById(id: string): Observable<CompletedTask> {
    return from(completedTaskService.getById(id))
  }
}
