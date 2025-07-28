import { Building } from '../models/Building'

export interface BuildingRepository {
  getAll(): Promise<Building[]>
}
