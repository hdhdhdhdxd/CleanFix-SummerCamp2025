import { PaginatedData } from '../models/PaginatedData'
import { Solicitation } from '../models/Solicitation'
import { SolicitationBrief } from '../models/SolicitationBrief'

export interface SolicitationRepository {
  getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Promise<PaginatedData<SolicitationBrief>>
  getById(id: number): Promise<Solicitation>
}
