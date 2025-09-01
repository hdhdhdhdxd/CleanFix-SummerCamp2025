import { CompletedTask } from '../models/CompletedTask'
import { CompletedTaskBrief } from '../models/CompletedTaskBrief'
import { PaginatedData } from '../models/PaginatedData'

export interface CompletedTaskRepository {
  create(
    solicitationId: number,
    incidenceId: number,
    companyId: number,
    isSolicitation: boolean,
    MaterialIds: number[],
  ): Promise<void>
  getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Promise<PaginatedData<CompletedTaskBrief>>
  getById(id: string): Promise<CompletedTask>
}
