import { Component, inject, OnInit, signal } from '@angular/core'
import { Table, TableColumn } from '../table/table'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'
import { IncidenceDialog } from '../incidence-dialog/incidence-dialog'

@Component({
  selector: 'app-incidences',
  imports: [Table, SearchBar, Pagination, IncidenceDialog],
  templateUrl: './incidences.html',
})
export class Incidences implements OnInit {
  private route = inject(ActivatedRoute)
  private location = inject(Location)
  private incidenceService = inject(IncidenceService)

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
    const currentParams = this.route.snapshot.queryParams
    const initialPageSize = +(currentParams['pageSize'] || 10)
    const initialPageNumber = +(currentParams['pageNumber'] || 1)
    const initialSearchTerm = currentParams['search'] || ''

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)
    this.searchTerm.set(initialSearchTerm)

    // Solo actualizar URL si no hay parámetros en la URL
    const hasParams =
      currentParams['pageSize'] || currentParams['pageNumber'] || currentParams['search']
    if (!hasParams) {
      this.updateUrl()
    }

    this.loadIncidences(this.pageNumber(), this.pageSize(), this.searchTerm())
  }

  private updateUrl(): void {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', this.pageSize().toString())
    queryParams.set('pageNumber', this.pageNumber().toString())

    const searchValue = this.searchTerm().trim()
    if (searchValue) {
      queryParams.set('search', searchValue)
    }

    const url = `${this.location.path().split('?')[0]}?${queryParams.toString()}`
    this.location.replaceState(url)
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
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadIncidences(1, $event, this.searchTerm())
  }

  onSearchChange(searchTerm: string) {
    const trimmedTerm = searchTerm.trim()
    this.searchTerm.set(trimmedTerm)
    this.pageNumber.set(1) // Reset to first page when searching
    this.updateUrl()
    this.loadIncidences(1, this.pageSize(), trimmedTerm || undefined)
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
