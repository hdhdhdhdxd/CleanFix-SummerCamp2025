import { Building } from '@/core/domain/models/Building'
import { BuildingRepository } from '@/core/domain/repositories/BuildingRepository'

const getAll = async (): Promise<Building[]> => {
  return [
    { id: '1', address: '123 Main St' },
    { id: '2', address: '456 Elm St' },
  ]
}

export const buildingApiRepository: BuildingRepository = {
  getAll,
}
