import { Material } from '@/core/domain/models/Material'
import { HttpParams } from '@angular/common/http'
import { environment } from 'src/environments/environment'

const getRandomThree = async (issueTypeId: number): Promise<Material[]> => {
  const queryParams = new HttpParams().set('issueTypeId', issueTypeId.toString())

  const response = await fetch(environment.baseUrl + `materials/random?${queryParams}`)

  if (!response.ok) {
    throw new Error('Error al obtener los materiales')
  }

  const responseJson: Material[] = await response.json()
  return responseJson
}

export const materialApiRepository = {
  getRandomThree,
}
