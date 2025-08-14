import { CurrencyPipe, DatePipe, NgClass, AsyncPipe } from '@angular/common'
import { Component, input } from '@angular/core'
import { Observable } from 'rxjs'

export interface TableColumn<T> {
  key: keyof T
  label: string
  type: 'text' | 'currency' | 'date' | 'number' | 'status'
}

@Component({
  selector: 'app-table',
  imports: [CurrencyPipe, DatePipe, NgClass, AsyncPipe],
  templateUrl: './table.html',
})
export class Table<
  T extends Record<string, string | number | Date> = Record<string, string | number | Date>,
> {
  data$ = input.required<Observable<T[]>>()
  tableColumns = input.required<TableColumn<T>[]>()

  get observableData() {
    return this.data$()
  }

  getColumns(): { key: string; label: string; type: string }[] {
    const customColumns = this.tableColumns()

    if (!customColumns || !(customColumns.length > 0)) {
      return []
    }

    return customColumns.map((column) => ({
      key: column.key as string,
      label: column.label,
      type: column.type,
    }))
  }

  getValue<K extends keyof T>(item: T, key: K): T[K] {
    return item[key]
  }

  getCurrencyValue<K extends keyof T>(item: T, key: K): number {
    return this.getValue(item, key) as number
  }

  getDateValue<K extends keyof T>(item: T, key: K): Date {
    return this.getValue(item, key) as Date
  }

  getStringValue<K extends keyof T>(item: T, key: K): string {
    return String(this.getValue(item, key))
  }

  getStatusClass(status: string): string {
    const statusStr = status.toLowerCase()
    switch (statusStr) {
      case 'pending':
        return 'bg-yellow-100 text-yellow-800'
      case 'in_progress':
        return 'bg-blue-100 text-blue-800'
      case 'completed':
        return 'bg-green-100 text-green-800'
      case 'canceled':
        return 'bg-red-100 text-red-800'
      default:
        return 'bg-gray-100 text-gray-800'
    }
  }

  hasData(items: T[] | null): boolean {
    return Boolean(items && items.length > 0)
  }
}
