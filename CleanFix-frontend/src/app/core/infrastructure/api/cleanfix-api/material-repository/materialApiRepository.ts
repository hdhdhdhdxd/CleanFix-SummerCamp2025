import { Material } from '@/core/domain/models/Material'
import { HttpParams } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { MaterialDto } from './MaterialDto'

const getRandomThree = async (issueTypeId: number): Promise<Material[]> => {
  const queryParams = new HttpParams().set('issueTypeId', issueTypeId.toString())

  const response = await fetch(environment.baseUrl + `/materials/random?${queryParams}`)

  if (!response.ok) {
    throw new Error('Error al obtener los materiales')
  }

  const responseJson: MaterialDto[] = await response.json()
  return responseJson.map((material) => ({
    id: material.id,
    name: material.name,
    cost: material.cost,
    costPerSquareMeter: material.costPerSquareMeter,
    available: material.available,
    issueTypeId: material.issueTypeId,
  }))
}

export const materialApiRepository = {
  getRandomThree,
}
