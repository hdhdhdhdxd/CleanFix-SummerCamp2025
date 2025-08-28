import { Component, inject } from '@angular/core'
import { Router } from '@angular/router'
import { ServiceCard } from '../service-card/service-card'

@Component({
  selector: 'app-service-section',
  imports: [ServiceCard],
  templateUrl: './service-section.html',
})
export class ServiceSection {
  router = inject(Router)
  goToChatbox() {
    this.router.navigate(['/service-request'])
  }
}
