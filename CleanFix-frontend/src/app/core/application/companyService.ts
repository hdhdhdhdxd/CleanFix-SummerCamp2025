import { CompanyRepository } from '../domain/repositories/CompanyRepository'

let companyRepository: CompanyRepository

const init = (repository: CompanyRepository) => {
  companyRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number) => {
  return companyRepository.getPaginated(pageNumber, pageSize)
}

export const companyService = {
  init,
  getPaginated,
}
