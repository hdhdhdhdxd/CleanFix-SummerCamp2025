import { Component, computed, inject, input, OnInit, signal } from '@angular/core'
import { Table, TableColumn } from '../table/table'
import { Incidence } from '@/core/domain/models/Incedence'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { ActivatedRoute, Router } from '@angular/router'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { toSignal } from '@angular/core/rxjs-interop'
import { map } from 'rxjs'
import { PaginationDto } from '@/core/domain/models/PaginationDto'

@Component({
  selector: 'app-incidences',
  imports: [Table, SearchBar, Pagination],
  templateUrl: './incidences.html',
})
export class Incidences implements OnInit {
  private router = inject(Router)
  private route = inject(ActivatedRoute)
  private incidenceService = inject(IncidenceService)

  totalPages = signal<number>(0)
  totalCount = signal<number>(0)
  hasPreviousPage = signal<boolean>(false)
  hasNextPage = signal<boolean>(false)

  pageSize = toSignal(this.route.queryParams.pipe(map((params) => +(params['pageSize'] || 10))), {
    initialValue: 10,
  })

  pageNumber = toSignal(
    this.route.queryParams.pipe(map((params) => +(params['pageNumber'] || 1))),
    { initialValue: 1 },
  )

  incidences$ = computed(() => {
    return this.incidenceService.getAll(this.pageNumber(), this.pageSize())
  })

  columns: TableColumn<Incidence>[] = [
    { key: 'id', label: 'ID', type: 'number' },
    { key: 'type', label: 'Dirección', type: 'text' },
    { key: 'date', label: 'Descripción', type: 'date' },
    { key: 'status', label: 'Tipo', type: 'text' },
    { key: 'description', label: 'Coste', type: 'text' },
    { key: 'apartmentId', label: 'Fecha', type: 'number' },
    { key: 'surface', label: 'Estado', type: 'number' },
    { key: 'priority', label: 'Estado', type: 'text' },
  ]

  ngOnInit() {
    const currentParams = this.route.snapshot.queryParams
    if (!currentParams['pageSize'] && !currentParams['pageNumber']) {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { pageSize: 10, pageNumber: 1 },
        replaceUrl: true,
      })
    }
  }

  handleTableResponse($event: PaginationDto<Incidence>) {
    this.totalPages.set($event.totalPages)
    this.totalCount.set($event.totalCount)
    this.hasPreviousPage.set($event.hasPreviousPage)
    this.hasNextPage.set($event.hasNextPage)
  }

  setPageNumber($event: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        pageSize: this.pageSize(),
        pageNumber: $event,
      },
    })
  }

  setPageSize($event: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        pageSize: $event,
        pageNumber: 1,
      },
    })
  }
}
