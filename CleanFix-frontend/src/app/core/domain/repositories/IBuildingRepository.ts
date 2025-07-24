import { Building } from '../models/Building';

export interface IBuildingRepository {
  getAllBuildings(): Promise<Building[]>;
}
