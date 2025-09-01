import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, inject, OnInit, signal } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { Pagination } from '../pagination/pagination'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'
import { Table, TableColumn } from '../table/table'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog, Pagination],
  templateUrl: './solicitations.html',
})
export class Solicitations implements OnInit {
  private route = inject(ActivatedRoute)
  private location = inject(Location)
  private solicitationService = inject(SolicitationService)

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
    const currentParams = this.route.snapshot.queryParams
    const initialPageSize = +(currentParams['pageSize'] || 10)
    const initialPageNumber = +(currentParams['pageNumber'] || 1)
    const initialSearchTerm = currentParams['search'] || ''

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)
    this.searchTerm.set(initialSearchTerm)

    const hasParams =
      currentParams['pageSize'] || currentParams['pageNumber'] || currentParams['search']
    if (!hasParams) {
      this.updateUrl()
    }

    this.loadSolicitations(this.pageNumber(), this.pageSize(), this.searchTerm())
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
    this.loadSolicitations($event, this.pageSize(), this.searchTerm())
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadSolicitations(1, $event, this.searchTerm())
  }

  onSearchChange(searchTerm: string) {
    const trimmedTerm = searchTerm.trim()
    this.searchTerm.set(trimmedTerm)
    this.pageNumber.set(1)
    this.updateUrl()
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
