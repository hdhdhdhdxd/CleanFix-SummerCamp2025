import { Company } from '../models/Company'

export interface CompanyRepository {
  getAll(): Promise<Company[]>
}
