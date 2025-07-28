import { BuildingRepository } from '@/core/domain/repositories/BuildingRepository'

let buildingRepository: BuildingRepository

const init = (repository: BuildingRepository) => {
  buildingRepository = repository
}

const getAll = async () => {
  return buildingRepository.getAll()
}

export const buildingService = {
  init,
  getAll,
}
