import { IncidenceRepository } from '../domain/repositories/IncidenceRepository'

let incidenceRepository: IncidenceRepository

const init = (repository: IncidenceRepository) => {
  incidenceRepository = repository
}

const getPaginated = async (pageNumber: number, pageSize: number, filterString?: string) => {
  return incidenceRepository.getPaginated(pageNumber, pageSize, filterString)
}

const getById = async (id: number) => {
  return incidenceRepository.getById(id)
}

export const incidenceService = {
  init,
  getPaginated,
  getById,
}
