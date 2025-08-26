import { Company } from '../models/Company'
import { PaginatedData } from '../models/PaginatedData'

export interface CompanyRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginatedData<Company>>
}
