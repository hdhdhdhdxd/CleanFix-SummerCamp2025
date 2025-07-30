import { Component } from '@angular/core'
import { ServiceCard } from '../service-card/service-card'

@Component({
  selector: 'app-service-section',
  imports: [ServiceCard],
  templateUrl: './service-section.html',
})
export class ServiceSection {}
