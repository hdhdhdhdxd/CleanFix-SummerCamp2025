import { Component, input, output, signal } from '@angular/core'

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
})
export class Pagination {
  pageSizeInput = input<number>()
  pageNumberInput = input<number>()
  totalItemsInput = input<number>()

  pageSize = signal<number>(5)
  pageNumber = signal<number>(1)
  totalItems = signal<number>(0)

  pageSizeChange = output<number>()
  pageNumberChange = output<number>()

  constructor() {
    const initialSize = this.pageSizeInput() ?? 5
    const initialNumber = this.pageNumberInput() ?? 1
    const initialTotal = this.totalItemsInput() ?? 0
    this.pageSize.set(initialSize)
    this.pageNumber.set(initialNumber)
    this.totalItems.set(initialTotal)
  }

  setPageSize($event: Event) {
    const target = $event.target as HTMLSelectElement
    const size = Number(target.value)
    if (!isNaN(size) && size > 0) {
      this.pageSize.set(size)
      this.pageSizeChange.emit(size)
    }
  }

  setPageNumber(num: number) {
    if (num < 1) return
    this.pageNumber.set(num)
    this.pageNumberChange.emit(num)
  }
}
