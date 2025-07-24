import { Building } from '@/core/domain/models/Building'
import { BuildingService } from '@/ui/services/building/building-service'
import { Component, OnInit, inject, signal } from '@angular/core'

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private buildingService = inject(BuildingService)

  buildings = signal<Building[]>([])

  async ngOnInit() {
    this.buildings.set(await this.buildingService.getAllBuildings())
  }
}
