import { buildingService } from '@/core/application/buildingService'
import { buildingApiRepository } from '@/core/infrastructure/repositories/buildingApiRepository'
import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class BuildingService {
  constructor() {
    buildingService.init(buildingApiRepository)
  }

  getAll() {
    return buildingService.getAll()
  }
}
