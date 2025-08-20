import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, computed, inject, OnInit, signal } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { toSignal } from '@angular/core/rxjs-interop'
import { Pagination } from '../pagination/pagination'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'
import { Table, TableColumn } from '../table/table'
import { PaginationDto } from '@/core/domain/models/PaginationDto'
import { map } from 'rxjs'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog, Pagination],
  templateUrl: './solicitations.html',
})
export class Solicitations implements OnInit {
  private router = inject(Router)
  private route = inject(ActivatedRoute)
  private solicitationService = inject(SolicitationService)

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
    if (!currentParams['pageSize'] && !currentParams['pageNumber']) {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: { pageSize: 10, pageNumber: 1 },
        replaceUrl: true,
      })
    }
  }

  handleTableResponse($event: PaginationDto<Solicitation>) {
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

  handleRowClick(item: Solicitation): void {
    this.selectedSolicitation = item
    this.showDialog = true
  }

  handleCloseDialog(): void {
    this.showDialog = false
    this.selectedSolicitation = null
  }
}
