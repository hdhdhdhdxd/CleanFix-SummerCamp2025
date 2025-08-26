import { Company } from '../models/Company'
import { PaginationDto } from '../models/PaginationDto'

export interface CompanyRepository {
  getPaginated(pageNumber: number, pageSize: number): Promise<PaginationDto<Company>>
}
