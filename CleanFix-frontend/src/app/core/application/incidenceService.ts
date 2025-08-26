import { IncidenceRepository } from '../domain/repositories/IncidenceRepository'

let incidenceRepository: IncidenceRepository

const init = (repository: IncidenceRepository) => {
  incidenceRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number) => {
  return incidenceRepository.getPaginated(pageNumber, pageSize)
}

export const incidenceService = {
  init,
  getPaginated,
}
