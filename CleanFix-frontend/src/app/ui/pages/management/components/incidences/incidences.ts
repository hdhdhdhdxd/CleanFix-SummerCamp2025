import { Component, computed, inject, input } from '@angular/core'
import { Table, TableColumn } from '../table/table'
import { Incidence } from '@/core/domain/models/Incedence'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { ActivatedRoute, Router } from '@angular/router'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { toSignal } from '@angular/core/rxjs-interop'

@Component({
  selector: 'app-incidences',
  imports: [Table, SearchBar, Pagination],
  templateUrl: './incidences.html',
})
export class Incidences {
  pageSize = input<number>(10)
  pageNumber = input<number>(1)

  incidenceService = inject(IncidenceService)
  router = inject(Router)
  route = inject(ActivatedRoute)

  incidencesSignal = toSignal(
    computed(() => this.incidenceService.getAll(this.pageNumber(), this.pageSize()))(),
  )

  incidences$ = computed(() => this.incidenceService.getAll(this.pageNumber(), this.pageSize()))

  setPageNumber($event: number) {
    this.router.navigate(['/management/incidences', this.pageSize(), $event])
  }

  setPageSize($event: number) {
    this.router.navigate(['/management/incidences', $event, 1])
    this.pageSize.apply($event)
  }

  selectedIncidence: Incidence | null = null
  showDialog = false

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

  handleRowClick(item: Incidence): void {
    this.selectedIncidence = item
    this.showDialog = true
  }
}
