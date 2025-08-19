import { PaginationDto } from '../models/PaginationDto'
import { Solicitation } from '../models/Solicitation'

export interface SolicitationRepository {
  getAll(): Promise<PaginationDto<Solicitation>>
}
