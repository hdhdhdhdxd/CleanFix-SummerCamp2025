import { materialService } from '@/core/application/materialService'
import { Material } from '@/core/domain/models/Material'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class MaterialService {
  getRandomThree(issueTypeId: number): Observable<Material[]> {
    return from(materialService.getRandomThree(issueTypeId))
  }
}
