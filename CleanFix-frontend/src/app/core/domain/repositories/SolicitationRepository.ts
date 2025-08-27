import { PaginatedData } from '../models/PaginatedData'
import { Solicitation } from '../models/Solicitation'
import { SolicitationBrief } from '../models/SolicitationBrief'

export interface SolicitationRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginatedData<SolicitationBrief>>
  getById(id: number): Promise<Solicitation>
}
