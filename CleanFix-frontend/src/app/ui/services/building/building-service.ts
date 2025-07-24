import { buildingService } from '@/core/application/buildingService'
import { buildingRepository } from '@/core/infrastructure/repositories/buildingRepository'
import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class BuildingService {
  constructor() {
    buildingService.init(buildingRepository)
  }

  getAllBuildings() {
    return buildingService.getAllBuildings()
  }
}
