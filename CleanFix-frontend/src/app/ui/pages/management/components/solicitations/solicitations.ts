import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, computed, inject, OnInit, signal } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { Pagination } from '../pagination/pagination'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'
import { Table, TableColumn } from '../table/table'
import { PaginationDto } from '@/core/domain/models/PaginationDto'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog, Pagination],
  templateUrl: './solicitations.html',
})
export class Solicitations implements OnInit {
  private route = inject(ActivatedRoute)
  private location = inject(Location)
  private solicitationService = inject(SolicitationService)

  totalPages = signal<number>(0)
  totalCount = signal<number>(0)
  hasPreviousPage = signal<boolean>(false)
  hasNextPage = signal<boolean>(false)

  pageSize = signal<number>(10)
  pageNumber = signal<number>(1)

  solicitations$ = computed(() => {
    return this.solicitationService.getAll(this.pageNumber(), this.pageSize())
  })

  selectedSolicitation: Solicitation | null = null
  showDialog = false

  columns: TableColumn<Solicitation>[] = [
    { key: 'id', label: 'ID', type: 'number' },
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'description', label: 'Descripción', type: 'text' },
    { key: 'type', label: 'Tipo', type: 'text' },
    { key: 'maintenanceCost', label: 'Coste', type: 'currency' },
    { key: 'date', label: 'Fecha', type: 'date' },
    { key: 'status', label: 'Estado', type: 'status' },
  ]

  ngOnInit() {
    const currentParams = this.route.snapshot.queryParams
    const initialPageSize = +(currentParams['pageSize'] || 10)
    const initialPageNumber = +(currentParams['pageNumber'] || 1)

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)

    if (!currentParams['pageSize'] && !currentParams['pageNumber']) {
      this.updateUrl()
    }
  }

  private updateUrl(): void {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', this.pageSize().toString())
    queryParams.set('pageNumber', this.pageNumber().toString())

    const url = `${this.location.path().split('?')[0]}?${queryParams.toString()}`
    this.location.replaceState(url)
  }

  handleTableResponse($event: PaginationDto<Solicitation>) {
    this.totalPages.set($event.totalPages)
    this.totalCount.set($event.totalCount)
    this.hasPreviousPage.set($event.hasPreviousPage)
    this.hasNextPage.set($event.hasNextPage)
  }

  setPageNumber($event: number) {
    this.pageNumber.set($event)
    this.updateUrl()
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
  }

  handleRowClick(item: Solicitation): void {
    this.selectedSolicitation = item
    this.showDialog = true
  }

  handleCloseDialog(): void {
    this.showDialog = false
    this.selectedSolicitation = null
  }
}
