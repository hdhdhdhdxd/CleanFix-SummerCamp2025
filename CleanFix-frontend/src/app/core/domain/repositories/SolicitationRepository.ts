import { PaginationDto } from '../models/PaginationDto'
import { Solicitation } from '../models/Solicitation'

export interface SolicitationRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginationDto<Solicitation>>
}
