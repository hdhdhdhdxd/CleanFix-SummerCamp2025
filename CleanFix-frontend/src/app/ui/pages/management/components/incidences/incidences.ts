import { Component, computed, inject, OnInit, signal } from '@angular/core'
import { Table, TableColumn } from '../table/table'
import { Incidence } from '@/core/domain/models/Incedence'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { PaginationDto } from '@/core/domain/models/PaginationDto'

@Component({
  selector: 'app-incidences',
  imports: [Table, SearchBar, Pagination],
  templateUrl: './incidences.html',
})
export class Incidences implements OnInit {
  private route = inject(ActivatedRoute)
  private location = inject(Location)
  private incidenceService = inject(IncidenceService)

  totalPages = signal<number>(0)
  totalCount = signal<number>(0)
  hasPreviousPage = signal<boolean>(false)
  hasNextPage = signal<boolean>(false)

  pageSize = signal<number>(10)
  pageNumber = signal<number>(1)

  incidences$ = computed(() => {
    return this.incidenceService.getAll(this.pageNumber(), this.pageSize())
  })

  columns: TableColumn<Incidence>[] = [
    { key: 'id', label: 'ID', type: 'number' },
    { key: 'type', label: 'Tipo', type: 'text' },
    { key: 'date', label: 'Fecha', type: 'date' },
    { key: 'status', label: 'Estado', type: 'text' },
    { key: 'description', label: 'Descripción', type: 'text' },
    { key: 'apartmentId', label: 'Apartamento', type: 'number' },
    { key: 'surface', label: 'Superficie', type: 'number' },
    { key: 'priority', label: 'Prioridad', type: 'text' },
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

  handleTableResponse($event: PaginationDto<Incidence>) {
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
}
