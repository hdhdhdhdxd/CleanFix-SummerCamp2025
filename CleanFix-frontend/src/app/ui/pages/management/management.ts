import { Component, inject, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core'
import { Router, NavigationEnd, RouterModule } from '@angular/router'
import { BuildingIcon } from '@/ui/shared/Icons/building-icon'
import { NgClass } from '@angular/common'
import { AlertIcon } from '@/ui/shared/Icons/alert-icon'
import { WrenchIcon } from '@/ui/shared/Icons/wrench-icon'
import { RouterOutlet } from '@angular/router'
import { HomeIcon } from '@/ui/shared/Icons/home-icon'
import { PaginationPersistence } from './services/pagination-persistence'

@Component({
  selector: 'app-management',
  imports: [BuildingIcon, NgClass, AlertIcon, WrenchIcon, RouterOutlet, RouterModule, HomeIcon],
  templateUrl: './management.html',
})
export class Management implements OnInit, AfterViewInit {
  @ViewChild('tabsContainer', { static: false }) tabsContainer!: ElementRef

  private router = inject(Router)
  private paginationPersistence = inject(PaginationPersistence)

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
      label: 'Historial de Trabajos',
      icon: 'wrench',
      route: '/management/completed-tasks',
    },
  ]

  currentRoute = ''
  activeTabIndex = 0

  ngOnInit(): void {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.urlAfterRedirects
        this.updateActiveTab()
      }
    })
    this.currentRoute = this.router.url
    this.updateActiveTab()
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.updateActiveTab(), 200)
  }

  isActive(tabRoute: string): boolean {
    return this.currentRoute.startsWith(tabRoute)
  }

  navigate(route: string, event: Event): void {
    event.preventDefault()

    // Intentar navegar al estado guardado, si no existe, navegar normalmente
    const wasRestored = this.paginationPersistence.navigateToSavedState(route)

    if (!wasRestored) {
      this.router.navigate([route])
    }
  }

  private updateActiveTab(): void {
    let activeIndex = this.tabs.findIndex((tab) => this.isActive(tab.route))

    if (activeIndex === -1) {
      activeIndex = 0
    }

    this.activeTabIndex = activeIndex
  }
}
