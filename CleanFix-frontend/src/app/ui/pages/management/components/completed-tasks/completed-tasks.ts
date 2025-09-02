import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { CompletedTaskService } from '@/ui/services/completedtask/completed-task-service'
import { Component, inject, OnInit, signal, OnDestroy } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { TableColumn, Table } from '../table/table'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { SearchBar } from '../search-bar/search-bar'
import { Pagination } from '../pagination/pagination'
import { CompletedTaskDialog } from '../completed-task-dialog/completed-task-dialog'
import { PaginationPersistence } from '../../services/pagination-persistence'

@Component({
  selector: 'app-completed-tasks',
  imports: [SearchBar, Table, Pagination, CompletedTaskDialog],
  templateUrl: './completed-tasks.html',
})
export class CompletedTasks implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute)
  private completedTaskService = inject(CompletedTaskService)
  private paginationPersistence = inject(PaginationPersistence)

  private readonly ROUTE_KEY = '/management/completed-tasks'

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
    // Primero verificar si hay estado guardado
    const savedState = this.paginationPersistence.getPageState(this.ROUTE_KEY)

    if (savedState) {
      // Usar estado guardado
      this.setInitialValues(savedState.pageSize, savedState.pageNumber, savedState.searchTerm)
    } else {
      // Usar parámetros de URL o valores por defecto
      const queryParams = this.route.snapshot.queryParams
      const pageSize = +(queryParams['pageSize'] || 5)
      const pageNumber = +(queryParams['pageNumber'] || 1)
      const searchTerm = queryParams['search'] || ''

      this.setInitialValues(pageSize, pageNumber, searchTerm)

      // Solo actualizar URL si no tiene parámetros completos
      if (!this.paginationPersistence.hasCompleteUrlParams(queryParams)) {
        this.updateUrl()
      }
    }

    this.loadCompletedTasks(this.pageNumber(), this.pageSize(), this.searchTerm())
  }

  ngOnDestroy() {
    this.saveCurrentState()
  }

  private setInitialValues(pageSize: number, pageNumber: number, searchTerm: string): void {
    this.pageSize.set(pageSize)
    this.pageNumber.set(pageNumber)
    this.searchTerm.set(searchTerm)
  }

  private saveCurrentState(): void {
    this.paginationPersistence.savePageState(
      this.ROUTE_KEY,
      this.pageNumber(),
      this.pageSize(),
      this.searchTerm(),
    )
  }

  private updateUrl(): void {
    this.paginationPersistence.updateCurrentUrl(
      this.ROUTE_KEY,
      this.pageNumber(),
      this.pageSize(),
      this.searchTerm(),
    )
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
    this.saveCurrentState()
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadCompletedTasks(1, $event, this.searchTerm())
    this.saveCurrentState()
  }

  onSearchChange(searchTerm: string) {
    const trimmedTerm = searchTerm.trim()
    this.searchTerm.set(trimmedTerm)
    this.pageNumber.set(1) // Reset to first page when searching
    this.updateUrl()
    this.loadCompletedTasks(1, this.pageSize(), trimmedTerm || undefined)
    this.saveCurrentState()
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
