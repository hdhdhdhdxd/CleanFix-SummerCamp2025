import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

let solicitationRepository: SolicitationRepository

const init = (repository: SolicitationRepository) => {
  solicitationRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number) => {
  return solicitationRepository.getPaginated(pageNumber, pageSize)
}

export const solicitationService = {
  init,
  getPaginated,
}
