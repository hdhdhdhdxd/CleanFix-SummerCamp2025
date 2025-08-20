import { Incidence } from '../models/Incedence'
import { PaginationDto } from '../models/PaginationDto'

export interface IncidenceRepository {
  getAll(pageNumber: number, pageSize: number): Promise<PaginationDto<Incidence>>
}
