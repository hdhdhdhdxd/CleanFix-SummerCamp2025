import { Component, inject, OnInit, signal } from '@angular/core'
import { Table, TableColumn } from '../table/table'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'

@Component({
  selector: 'app-incidences',
  imports: [Table, SearchBar, Pagination],
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

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)

    if (!currentParams['pageSize'] && !currentParams['pageNumber']) {
      this.updateUrl()
    }
    this.loadIncidences(this.pageNumber(), this.pageSize())
  }

  private updateUrl(): void {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', this.pageSize().toString())
    queryParams.set('pageNumber', this.pageNumber().toString())

    const url = `${this.location.path().split('?')[0]}?${queryParams.toString()}`
    this.location.replaceState(url)
  }

  private loadIncidences(page: number, pageSize: number) {
    this.incidenceService.getPaginated(page, pageSize).subscribe((result) => {
      this.updateValues(result)
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
    this.loadIncidences($event, this.pageSize())
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadIncidences(1, $event)
  }
}
