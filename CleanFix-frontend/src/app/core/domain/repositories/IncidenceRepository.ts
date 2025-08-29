import { IncidenceBrief } from '../models/IncidenceBrief'
import { PaginatedData } from '../models/PaginatedData'

export interface IncidenceRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginatedData<IncidenceBrief>>
}
