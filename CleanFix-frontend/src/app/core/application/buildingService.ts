import { IBuildingRepository } from '@/core/domain/repositories/IBuildingRepository';

let buildingRepository: IBuildingRepository;

const init = (repository: IBuildingRepository) => {
  buildingRepository = repository;
};

const getAllBuildings = async () => {
  return buildingRepository.getAllBuildings();
};

export const buildingService = {
  init,
  getAllBuildings,
};
