import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

let solicitationRepository: SolicitationRepository

const init = (repository: SolicitationRepository) => {
  solicitationRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number, filterString?: string) => {
  return solicitationRepository.getPaginated(pageNumber, pageSize, filterString)
}

const getById = async (id: number) => {
  return solicitationRepository.getById(id)
}

export const solicitationService = {
  init,
  getPaginated,
  getById,
}
