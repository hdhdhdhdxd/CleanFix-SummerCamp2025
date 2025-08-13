import { CompanyRepository } from '../domain/repositories/CompanyRepository'

let companyRepository: CompanyRepository

const init = (repository: CompanyRepository) => {
  companyRepository = repository
}

const getAll = async () => {
  return companyRepository.getAll()
}

export const companyService = {
  init,
  getAll,
}
