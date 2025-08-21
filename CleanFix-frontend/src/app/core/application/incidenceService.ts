import { IncidenceRepository } from '../domain/repositories/IncidenceRepository'

let incidenceRepository: IncidenceRepository

const init = (repository: IncidenceRepository) => {
  incidenceRepository = repository
}

const getAll = async (pageNumber: number, pageSize: number) => {
  return incidenceRepository.getAll(pageNumber, pageSize)
}

export const incidenceService = {
  init,
  getAll,
}
