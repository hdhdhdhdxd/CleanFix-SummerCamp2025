import { Component, input, output } from '@angular/core'

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
})
export class Pagination {
  pageSize = input<number>()
  pageNumber = input<number>()
  totalPages = input<number>()
  totalCount = input<number>()
  hasPreviousPage = input<boolean>()
  hasNextPage = input<boolean>()

  pageSizeChange = output<number>()
  pageNumberChange = output<number>()

  setPageSize(size: number) {
    this.pageSizeChange.emit(size)
  }

  setPageNumber(page: number) {
    this.pageNumberChange.emit(page)
  }
}
