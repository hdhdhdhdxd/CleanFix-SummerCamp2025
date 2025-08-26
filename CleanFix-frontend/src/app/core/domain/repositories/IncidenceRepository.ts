import { Incidence } from '../models/Incedence'
import { PaginatedData } from '../models/PaginatedData'

export interface IncidenceRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginatedData<Incidence>>
}
