import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

let solicitationRepository: SolicitationRepository

const init = (repository: SolicitationRepository) => {
  solicitationRepository = repository
}

const getAll = async () => {
  return solicitationRepository.getAll()
}

export const solicitationService = {
  init,
  getAll,
}
