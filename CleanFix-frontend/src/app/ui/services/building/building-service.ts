import { buildingService } from '@/core/application/buildingService'
import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class BuildingService {
  getAll() {
    return buildingService.getAll()
  }
}
