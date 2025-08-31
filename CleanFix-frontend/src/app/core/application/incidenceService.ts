import { IncidenceRepository } from '../domain/repositories/IncidenceRepository'

let incidenceRepository: IncidenceRepository

const init = (repository: IncidenceRepository) => {
  incidenceRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number) => {
  return incidenceRepository.getPaginated(pageNumber, pageSize)
}

const getById = async (id: number) => {
  return incidenceRepository.getById(id)
}

export const incidenceService = {
  init,
  getPaginated,
  getById,
}
