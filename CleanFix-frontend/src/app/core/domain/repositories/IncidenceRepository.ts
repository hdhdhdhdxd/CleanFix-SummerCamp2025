import { Incidence } from '../models/Incedence'
import { PaginationDto } from '../models/PaginationDto'

export interface IncidenceRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginationDto<Incidence>>
}
