import { Component, inject, OnInit } from '@angular/core'
import { Router, NavigationEnd, RouterModule } from '@angular/router'
import { BuildingIcon } from '@/ui/shared/Icons/building-icon'
import { NgClass } from '@angular/common'
import { AlertIcon } from '@/ui/shared/Icons/alert-icon'
import { WrenchIcon } from '@/ui/shared/Icons/wrench-icon'
import { RouterOutlet } from '@angular/router'

@Component({
  selector: 'app-management',
  imports: [BuildingIcon, NgClass, AlertIcon, WrenchIcon, RouterOutlet, RouterModule],
  templateUrl: './management.html',
})
export class Management implements OnInit {
  tabs = [
    {
      label: 'Solicitudes',
      icon: 'building',
      route: '/management/solicitations',
    },
    {
      label: 'Incidencias',
      icon: 'alert',
      route: '/management/incidences',
    },
    {
      label: 'Trabajos en Proceso',
      icon: 'wrench',
      route: '/management/requests',
    },
  ]

  router = inject(Router)
  currentRoute = ''

  ngOnInit(): void {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.urlAfterRedirects
      }
    })
    this.currentRoute = this.router.url
  }

  isActive(tabRoute: string): boolean {
    return this.currentRoute.startsWith(tabRoute)
  }
}
