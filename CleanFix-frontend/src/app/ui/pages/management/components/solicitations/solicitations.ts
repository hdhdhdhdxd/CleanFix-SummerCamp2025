import { Component, inject } from '@angular/core'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Table, TableColumn } from '../table/table'
import { Solicitation } from '@/core/domain/models/Solicitation'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table],
  templateUrl: './solicitations.html',
})
export class Solicitations {
  solicitationService = inject(SolicitationService)

  solicitations$ = this.solicitationService.getAll()

  columns: TableColumn<Solicitation>[] = [
    { key: 'id', label: 'ID', type: 'number' },
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'description', label: 'Descripción', type: 'text' },
    { key: 'type', label: 'Tipo', type: 'text' },
    { key: 'cost', label: 'Coste', type: 'currency' },
    { key: 'date', label: 'Fecha', type: 'date' },
    { key: 'status', label: 'Estado', type: 'status' },
  ]
}
