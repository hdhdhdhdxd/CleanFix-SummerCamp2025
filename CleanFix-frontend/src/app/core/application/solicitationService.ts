import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

let solicitationRepository: SolicitationRepository

const init = (repository: SolicitationRepository) => {
  solicitationRepository = repository
}

const getAll = async (pageNumber: number, pageSize: number) => {
  return solicitationRepository.getAll(pageNumber, pageSize)
}

export const solicitationService = {
  init,
  getAll,
}
