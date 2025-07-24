import { Building } from '@/core/domain/models/Building';
import { IBuildingRepository } from '@/core/domain/repositories/IBuildingRepository';

const getAllBuildings = async (): Promise<Building[]> => {
  return [
    { id: '1', address: '123 Main St' },
    { id: '2', address: '456 Elm St' },
  ];
};

export const buildingRepository: IBuildingRepository = {
  getAllBuildings,
};
