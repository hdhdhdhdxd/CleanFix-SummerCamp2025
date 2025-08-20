import { Component, input, output, computed } from '@angular/core'
import { PaginationDto } from '@/core/domain/models/PaginationDto'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { Incidence } from '@/core/domain/models/Incedence'

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
})
export class Pagination {
  paginationData = input<PaginationDto<Solicitation | Incidence>>()
  currentPageSize = input<number>(10)
  currentPageNumber = input<number>(1)

  pageSize = computed(() => this.currentPageSize())
  pageNumber = computed(() => this.currentPageNumber())
  totalItems = computed(() => this.paginationData()?.totalCount ?? 0)
  totalPages = computed(() => this.paginationData()?.totalPages ?? 1)
  hasPreviousPage = computed(() => this.paginationData()?.hasPreviousPage ?? false)
  hasNextPage = computed(() => this.paginationData()?.hasNextPage ?? false)

  startItem = computed(() => {
    const data = this.paginationData()
    if (!data || data.totalCount === 0) return 0
    return (this.pageNumber() - 1) * this.pageSize() + 1
  })

  endItem = computed(() => {
    const data = this.paginationData()
    if (!data || data.totalCount === 0) return 0
    const itemsInCurrentPage = data.items.length
    return (this.pageNumber() - 1) * this.pageSize() + itemsInCurrentPage
  })

  canGoPrevious = computed(() => this.hasPreviousPage())
  canGoNext = computed(() => this.hasNextPage())

  pageSizeChange = output<number>()
  pageNumberChange = output<number>()

  setPageSize($event: Event) {
    const target = $event.target as HTMLSelectElement
    const size = Number(target.value)
    if (!isNaN(size) && size > 0) {
      this.pageSizeChange.emit(size)
    }
  }

  setPageNumber(num: number) {
    if (num < 1 || num > this.totalPages()) return
    this.pageNumberChange.emit(num)
  }
}
