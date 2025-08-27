import { Material } from '@/core/domain/models/Material'
import { materialApiRepository } from '@/core/infrastructure/api/cleanfix-api/material-repository/materialApiRepository'
import { Injectable } from '@angular/core'
import { from, Observable } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class MaterialService {
  getRandomThree(issueTypeId: number): Observable<Material[]> {
    return from(materialApiRepository.getRandomThree(issueTypeId))
  }
}
