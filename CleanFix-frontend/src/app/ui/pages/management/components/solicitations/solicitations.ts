import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, computed, inject, input } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { toSignal } from '@angular/core/rxjs-interop'
import { Pagination } from '../pagination/pagination'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'
import { Table, TableColumn } from '../table/table'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog, Pagination],
  templateUrl: './solicitations.html',
})
export class Solicitations {
  pageSize = input<number>(10)
  pageNumber = input<number>(1)

  router = inject(Router)
  route = inject(ActivatedRoute)
  solicitationService = inject(SolicitationService)

  solicitationsSignal = toSignal(
    computed(() => this.solicitationService.getAll(this.pageNumber(), this.pageSize()))(),
  )

  solicitations$ = computed(() =>
    this.solicitationService.getAll(this.pageNumber(), this.pageSize()),
  )

  setPageNumber($event: number) {
    this.router.navigate(['/management/solicitations', this.pageSize(), $event])
  }

  setPageSize($event: number) {
    this.router.navigate(['/management/solicitations', $event, 1])
    this.pageSize.apply($event)
  }

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

  handleRowClick(item: Solicitation): void {
    this.selectedSolicitation = item
    this.showDialog = true
  }

  handleCloseDialog(): void {
    this.showDialog = false
    this.selectedSolicitation = null
  }
}
