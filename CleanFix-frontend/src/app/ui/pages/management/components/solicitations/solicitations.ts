import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, computed, inject, signal } from '@angular/core'
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
  pageSize = signal<number>(5)
  pageNumber = signal<number>(1)

  solicitationService = inject(SolicitationService)

  solicitations$ = computed(() =>
    this.solicitationService.getAll(this.pageNumber(), this.pageSize()),
  )

  setPageNumber($event: number) {
    this.pageNumber.set($event)
  }
  setPageSize($event: number) {
    this.pageSize.set($event)
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
