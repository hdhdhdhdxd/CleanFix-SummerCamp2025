import { Component, input, output } from '@angular/core'

export interface PaginationProps {
  pageNumber: number
  pageSize: number
  totalPages: number
  totalCount: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
})
export class Pagination {
  props = input.required<PaginationProps>()

  pageSizeChange = output<number>()
  pageNumberChange = output<number>()

  setPageSize(size: number) {
    this.pageSizeChange.emit(size)
  }

  setPageNumber(page: number) {
    this.pageNumberChange.emit(page)
  }
}
