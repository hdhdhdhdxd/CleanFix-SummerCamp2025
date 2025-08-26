import { PaginatedData } from '../models/PaginatedData'
import { Solicitation } from '../models/Solicitation'

export interface SolicitationRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginatedData<Solicitation>>
}
