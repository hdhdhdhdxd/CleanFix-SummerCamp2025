import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { CompletedTaskService } from '@/ui/services/completedtask/completed-task-service'
import { Component, inject, OnInit, signal } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { TableColumn, Table } from '../table/table'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { Location } from '@angular/common'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { CompletedTaskDialog } from '../completed-task-dialog/completed-task-dialog'

@Component({
  selector: 'app-completed-tasks',
  imports: [SearchBar, Table, Pagination, CompletedTaskDialog],
  templateUrl: './completed-tasks.html',
})
export class CompletedTasks implements OnInit {
  private route = inject(ActivatedRoute)
  private location = inject(Location)
  private completedTaskService = inject(CompletedTaskService)

  completedTasks = signal<CompletedTaskBrief[]>([])
  totalPages = signal<number>(0)
  totalCount = signal<number>(0)
  hasPreviousPage = signal<boolean>(false)
  hasNextPage = signal<boolean>(false)

  pageSize = signal<number>(5)
  pageNumber = signal<number>(1)

  completedTaskId: number | null = null
  showDialog = signal<boolean>(false)

  columns: TableColumn<CompletedTaskBrief>[] = [
    {
      key: 'isSolicitation',
      label: 'Tipo de Trabajo',
      type: 'boolean',
      labelTrue: 'Solicitud',
      labelFalse: 'Incidencia',
    },
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'companyName', label: 'Empresa', type: 'text' },
    { key: 'issueType', label: 'Tipo', type: 'text' },
    { key: 'creationDate', label: 'Fecha de Creación', type: 'date' },
    {
      key: 'completionDate',
      label: 'Progreso',
      type: 'progress',
      keyStart: 'creationDate',
      keyEnd: 'completionDate',
    },
  ]

  ngOnInit() {
    const currentParams = this.route.snapshot.queryParams
    const initialPageSize = +(currentParams['pageSize'] || 5)
    const initialPageNumber = +(currentParams['pageNumber'] || 1)

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)

    if (!currentParams['pageSize'] && !currentParams['pageNumber']) {
      this.updateUrl()
    }
    this.loadCompletedTasks(this.pageNumber(), this.pageSize())
  }

  private updateUrl(): void {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', this.pageSize().toString())
    queryParams.set('pageNumber', this.pageNumber().toString())

    const url = `${this.location.path().split('?')[0]}?${queryParams.toString()}`
    this.location.replaceState(url)
  }

  private loadCompletedTasks(page: number, pageSize: number) {
    this.completedTaskService.getPaginated(page, pageSize).subscribe((result) => {
      this.updateValues(result)
    })
  }

  private updateValues(pagination: PaginatedData<CompletedTaskBrief>) {
    this.completedTasks.set(pagination.items)
    this.pageNumber.set(pagination.pageNumber)
    this.totalPages.set(pagination.totalPages)
    this.totalCount.set(pagination.totalCount)
    this.hasPreviousPage.set(pagination.hasPreviousPage)
    this.hasNextPage.set(pagination.hasNextPage)
  }

  setPageNumber($event: number) {
    this.pageNumber.set($event)
    this.updateUrl()
    this.loadCompletedTasks($event, this.pageSize())
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadCompletedTasks(1, $event)
  }

  handleRowClick(item: CompletedTaskBrief): void {
    this.completedTaskId = item.id
    this.showDialog.set(true)
  }

  handleCloseDialog(): void {
    this.completedTaskId = null
    this.showDialog.set(false)
  }
}
