import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

let solicitationRepository: SolicitationRepository

const init = (repository: SolicitationRepository) => {
  solicitationRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number) => {
  return solicitationRepository.getPaginated(pageNumber, pageSize)
}

const getById = async (id: number) => {
  return solicitationRepository.getById(id)
}

export const solicitationService = {
  init,
  getPaginated,
  getById,
}
