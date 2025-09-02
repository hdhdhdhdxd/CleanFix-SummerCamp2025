import { Component, inject, OnInit, signal, OnDestroy } from '@angular/core'
import { Table, TableColumn } from '../table/table'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { ActivatedRoute } from '@angular/router'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'
import { IncidenceDialog } from '../incidence-dialog/incidence-dialog'
import { PaginationPersistence } from '../../services/pagination-persistence'

@Component({
  selector: 'app-incidences',
  imports: [Table, SearchBar, Pagination, IncidenceDialog],
  templateUrl: './incidences.html',
})
export class Incidences implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute)
  private incidenceService = inject(IncidenceService)
  private paginationPersistence = inject(PaginationPersistence)

  private readonly ROUTE_KEY = '/management/incidences'

  incidences = signal<IncidenceBrief[]>([])
  totalPages = signal<number>(0)
  totalCount = signal<number>(0)
  hasPreviousPage = signal<boolean>(false)
  hasNextPage = signal<boolean>(false)

  pageSize = signal<number>(10)
  pageNumber = signal<number>(1)
  searchTerm = signal<string>('')

  isLoading = signal<boolean>(false)
  hasError = signal<boolean>(false)
  errorMessage = signal<string>('')

  incidenceId: number | null = null
  showDialog = signal<boolean>(false)

  columns: TableColumn<IncidenceBrief>[] = [
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'date', label: 'Fecha', type: 'date' },
    { key: 'issueType', label: 'Tipo', type: 'text' },
    { key: 'priority', label: 'Prioridad', type: 'text' },
  ]

  ngOnInit() {
    // Primero verificar si hay estado guardado
    const savedState = this.paginationPersistence.getPageState(this.ROUTE_KEY)

    if (savedState) {
      // Usar estado guardado
      this.setInitialValues(savedState.pageSize, savedState.pageNumber, savedState.searchTerm)
    } else {
      // Usar parámetros de URL o valores por defecto
      const queryParams = this.route.snapshot.queryParams
      const pageSize = +(queryParams['pageSize'] || 10)
      const pageNumber = +(queryParams['pageNumber'] || 1)
      const searchTerm = queryParams['search'] || ''

      this.setInitialValues(pageSize, pageNumber, searchTerm)
    }

    // Solo actualizar URL si no tiene parámetros completos
    const queryParams = this.route.snapshot.queryParams
    if (!this.paginationPersistence.hasCompleteUrlParams(queryParams)) {
      this.updateUrl()
    }

    this.loadIncidences(this.pageNumber(), this.pageSize(), this.searchTerm())
  }

  ngOnDestroy() {
    this.saveCurrentState()
  }

  private setInitialValues(pageSize: number, pageNumber: number, searchTerm: string): void {
    this.pageSize.set(pageSize)
    this.pageNumber.set(pageNumber)
    this.searchTerm.set(searchTerm)
  }

  private saveCurrentState(): void {
    this.paginationPersistence.savePageState(
      this.ROUTE_KEY,
      this.pageNumber(),
      this.pageSize(),
      this.searchTerm(),
    )
  }

  private updateUrl(): void {
    this.paginationPersistence.updateCurrentUrl(
      this.ROUTE_KEY,
      this.pageNumber(),
      this.pageSize(),
      this.searchTerm(),
    )
  }

  private loadIncidences(page: number, pageSize: number, searchTerm?: string) {
    const filterString = searchTerm?.trim() || undefined

    this.isLoading.set(true)
    this.hasError.set(false)
    this.errorMessage.set('')

    this.incidenceService.getPaginated(page, pageSize, filterString).subscribe({
      next: (result) => {
        this.updateValues(result)
        this.isLoading.set(false)
      },
      error: (error) => {
        console.error('Error loading incidences:', error)
        this.isLoading.set(false)
        this.hasError.set(true)
        this.errorMessage.set('Error al cargar las incidencias. Por favor, inténtelo de nuevo.')
        this.incidences.set([])
      },
    })
  }

  private updateValues(pagination: PaginatedData<IncidenceBrief>) {
    this.incidences.set(pagination.items)
    this.pageNumber.set(pagination.pageNumber)
    this.totalPages.set(pagination.totalPages)
    this.totalCount.set(pagination.totalCount)
    this.hasPreviousPage.set(pagination.hasPreviousPage)
    this.hasNextPage.set(pagination.hasNextPage)
  }

  setPageNumber($event: number) {
    this.pageNumber.set($event)
    this.updateUrl()
    this.loadIncidences($event, this.pageSize(), this.searchTerm())
    this.saveCurrentState()
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadIncidences(1, $event, this.searchTerm())
    this.saveCurrentState()
  }

  onSearchChange(searchTerm: string) {
    const trimmedTerm = searchTerm.trim()
    this.searchTerm.set(trimmedTerm)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadIncidences(1, this.pageSize(), trimmedTerm || undefined)
    this.saveCurrentState()
  }

  handleRowClick(item: IncidenceBrief): void {
    this.incidenceId = item.id
    this.showDialog.set(true)
  }

  handleCloseDialog(): void {
    this.incidenceId = null
    this.showDialog.set(false)
  }

  handleTaskCreated(): void {
    this.loadIncidences(this.pageNumber(), this.pageSize(), this.searchTerm())
  }
}
