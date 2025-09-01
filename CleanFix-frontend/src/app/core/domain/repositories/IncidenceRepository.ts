import { Incidence } from '../models/Incidence'
import { IncidenceBrief } from '../models/IncidenceBrief'
import { PaginatedData } from '../models/PaginatedData'

export interface IncidenceRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginatedData<IncidenceBrief>>
  getById(id: number): Promise<Incidence>
}
