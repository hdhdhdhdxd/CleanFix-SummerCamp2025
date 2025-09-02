import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, inject, OnInit, signal, OnDestroy } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Pagination } from '../pagination/pagination'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'
import { Table, TableColumn } from '../table/table'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'
import { PaginationPersistence } from '../../services/pagination-persistence'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog, Pagination],
  templateUrl: './solicitations.html',
})
export class Solicitations implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute)
  private solicitationService = inject(SolicitationService)
  private paginationPersistence = inject(PaginationPersistence)

  private readonly ROUTE_KEY = '/management/solicitations'

  solicitations = signal<SolicitationBrief[]>([])
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

  solicitationId: number | null = null
  showDialog = signal<boolean>(false)

  columns: TableColumn<SolicitationBrief>[] = [
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'date', label: 'Fecha', type: 'date' },
    { key: 'issueType', label: 'Tipo', type: 'text' },
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

    this.loadSolicitations(this.pageNumber(), this.pageSize(), this.searchTerm())
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

  private loadSolicitations(page: number, pageSize: number, searchTerm?: string) {
    const filterString = searchTerm?.trim() || undefined

    this.isLoading.set(true)
    this.hasError.set(false)
    this.errorMessage.set('')

    this.solicitationService.getPaginated(page, pageSize, filterString).subscribe({
      next: (result) => {
        this.updateValues(result)
        this.isLoading.set(false)
      },
      error: (error) => {
        console.error('Error loading solicitations:', error)
        this.isLoading.set(false)
        this.hasError.set(true)
        this.errorMessage.set('Error al cargar las solicitudes. Por favor, inténtelo de nuevo.')
        this.solicitations.set([])
      },
    })
  }

  private updateValues(pagination: PaginatedData<SolicitationBrief>) {
    this.solicitations.set(pagination.items)
    this.pageNumber.set(pagination.pageNumber)
    this.totalPages.set(pagination.totalPages)
    this.totalCount.set(pagination.totalCount)
    this.hasPreviousPage.set(pagination.hasPreviousPage)
    this.hasNextPage.set(pagination.hasNextPage)
  }

  setPageNumber($event: number) {
    this.pageNumber.set($event)
    this.updateUrl()
    this.saveCurrentState()
    this.loadSolicitations($event, this.pageSize(), this.searchTerm())
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.saveCurrentState()
    this.loadSolicitations(1, $event, this.searchTerm())
  }

  onSearchChange(searchTerm: string) {
    const trimmedTerm = searchTerm.trim()
    this.searchTerm.set(trimmedTerm)
    this.pageNumber.set(1)
    this.updateUrl()
    this.saveCurrentState()
    this.loadSolicitations(1, this.pageSize(), trimmedTerm || undefined)
  }

  handleRowClick(item: SolicitationBrief): void {
    this.solicitationId = item.id
    this.showDialog.set(true)
  }

  handleCloseDialog(): void {
    this.solicitationId = null
    this.showDialog.set(false)
  }

  handleTaskCreated(): void {
    this.loadSolicitations(this.pageNumber(), this.pageSize(), this.searchTerm())
  }
}
