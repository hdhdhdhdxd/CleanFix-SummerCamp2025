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
  searchTerm = signal<string>('')

  isLoading = signal<boolean>(false)
  hasError = signal<boolean>(false)
  errorMessage = signal<string>('')

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
    const initialSearchTerm = currentParams['search'] || ''

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)
    this.searchTerm.set(initialSearchTerm)

    // Solo actualizar URL si no hay parámetros en la URL
    const hasParams =
      currentParams['pageSize'] || currentParams['pageNumber'] || currentParams['search']
    if (!hasParams) {
      this.updateUrl()
    }

    this.loadCompletedTasks(this.pageNumber(), this.pageSize(), this.searchTerm())
  }

  private updateUrl(): void {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', this.pageSize().toString())
    queryParams.set('pageNumber', this.pageNumber().toString())

    const searchValue = this.searchTerm().trim()
    if (searchValue) {
      queryParams.set('search', searchValue)
    }

    const url = `${this.location.path().split('?')[0]}?${queryParams.toString()}`
    this.location.replaceState(url)
  }

  private loadCompletedTasks(page: number, pageSize: number, searchTerm?: string) {
    const filterString = searchTerm?.trim() || undefined

    this.isLoading.set(true)
    this.hasError.set(false)
    this.errorMessage.set('')

    this.completedTaskService.getPaginated(page, pageSize, filterString).subscribe({
      next: (result) => {
        this.updateValues(result)
        this.isLoading.set(false)
      },
      error: (error) => {
        console.error('Error loading completed tasks:', error)
        this.isLoading.set(false)
        this.hasError.set(true)
        this.errorMessage.set(
          'Error al cargar las tareas completadas. Por favor, inténtelo de nuevo.',
        )
        this.completedTasks.set([])
      },
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
    this.loadCompletedTasks($event, this.pageSize(), this.searchTerm())
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadCompletedTasks(1, $event, this.searchTerm())
  }

  onSearchChange(searchTerm: string) {
    const trimmedTerm = searchTerm.trim()
    this.searchTerm.set(trimmedTerm)
    this.pageNumber.set(1) // Reset to first page when searching
    this.updateUrl()
    this.loadCompletedTasks(1, this.pageSize(), trimmedTerm || undefined)
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
