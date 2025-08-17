import { Component, inject } from '@angular/core'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Table, TableColumn } from '../table/table'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog],
  templateUrl: './solicitations.html',
})
export class Solicitations {
  solicitationService = inject(SolicitationService)

  solicitations$ = this.solicitationService.getAll()

  selectedSolicitation: Solicitation | null = null
  showDialog = false

  columns: TableColumn<Solicitation>[] = [
    { key: 'id', label: 'ID', type: 'number' },
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'description', label: 'Descripción', type: 'text' },
    { key: 'type', label: 'Tipo', type: 'text' },
    { key: 'cost', label: 'Coste', type: 'currency' },
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
