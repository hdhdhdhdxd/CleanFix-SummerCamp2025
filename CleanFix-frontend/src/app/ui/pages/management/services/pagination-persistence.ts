import { Injectable, inject } from '@angular/core'
import { Router, NavigationEnd } from '@angular/router'
import { Location } from '@angular/common'
import { filter } from 'rxjs/operators'

interface PageState {
  url: string
  pageNumber: number
  pageSize: number
  searchTerm: string
}

@Injectable({
  providedIn: 'root',
})
export class PaginationPersistence {
  private router = inject(Router)
  private location = inject(Location)
  private managementPages = new Map<string, PageState>()
  private isInManagement = false

  constructor() {
    // Monitorear navegación para limpiar cuando se salga de management
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const isCurrentlyInManagement = event.urlAfterRedirects.startsWith('/management')

        if (this.isInManagement && !isCurrentlyInManagement) {
          // Se está saliendo de management, limpiar datos
          this.clearAllStates()
        }

        this.isInManagement = isCurrentlyInManagement
      })
  }

  /**
   * Guarda el estado de una página de management
   */
  savePageState(route: string, pageNumber: number, pageSize: number, searchTerm: string): void {
    if (!route.startsWith('/management')) return

    this.managementPages.set(route, {
      url: this.buildUrl(route, pageNumber, pageSize, searchTerm),
      pageNumber,
      pageSize,
      searchTerm,
    })
  }

  /**
   * Construye la URL con parámetros
   */
  buildUrl(route: string, pageNumber: number, pageSize: number, searchTerm: string): string {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', pageSize.toString())
    queryParams.set('pageNumber', pageNumber.toString())

    if (searchTerm.trim()) {
      queryParams.set('search', searchTerm.trim())
    }

    return `${route}?${queryParams.toString()}`
  }

  /**
   * Verifica si la URL tiene los parámetros de paginación completos
   */
  hasCompleteUrlParams(queryParams: Record<string, string>): boolean {
    return !!(queryParams['pageSize'] && queryParams['pageNumber'])
  }

  /**
   * Actualiza la URL actual con los parámetros de paginación
   */
  updateCurrentUrl(route: string, pageNumber: number, pageSize: number, searchTerm: string): void {
    const url = this.buildUrl(route, pageNumber, pageSize, searchTerm)
    const basePath = route
    const fullUrl = `${basePath}?${url.split('?')[1]}`
    this.location.replaceState(fullUrl)
  }

  /**
   * Obtiene el estado guardado de una página
   */
  getPageState(route: string): PageState | null {
    return this.managementPages.get(route) || null
  }

  /**
   * Navega a una página restaurando su estado previo
   */
  navigateToSavedState(route: string): boolean {
    const savedState = this.getPageState(route)

    if (savedState) {
      this.router.navigateByUrl(savedState.url)
      return true
    }

    return false
  }

  /**
   * Limpia todos los estados guardados
   */
  clearAllStates(): void {
    this.managementPages.clear()
  }
}
