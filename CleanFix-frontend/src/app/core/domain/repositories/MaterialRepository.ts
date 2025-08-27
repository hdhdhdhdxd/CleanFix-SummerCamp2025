import { Material } from '../models/Material'

export interface MaterialRepository {
  getRandomThree(issueTypeId: number): Promise<Material[]>
}
