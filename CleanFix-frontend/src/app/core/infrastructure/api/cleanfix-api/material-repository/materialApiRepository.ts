import { Material } from '@/core/domain/models/Material'
import { HttpClient, HttpParams } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { MaterialDto } from './MaterialDto'
import { firstValueFrom } from 'rxjs'
import { MaterialRepository } from '@/core/domain/repositories/MaterialRepository'

export class MaterialApiRepository implements MaterialRepository {
  constructor(private http: HttpClient) {}

  async getRandomThree(issueTypeId: number): Promise<Material[]> {
    const params = new HttpParams().set('issueTypeId', issueTypeId.toString())
    const responseJson = await firstValueFrom(
      this.http.get<MaterialDto[]>(environment.baseUrl + 'materials/random', {
        params,
        withCredentials: true,
      }),
    )
    return responseJson.map((material) => ({
      id: material.id,
      name: material.name,
      cost: material.cost,
      costPerSquareMeter: material.costPerSquareMeter,
      available: material.available,
      issueTypeId: material.issueTypeId,
    }))
  }
}
