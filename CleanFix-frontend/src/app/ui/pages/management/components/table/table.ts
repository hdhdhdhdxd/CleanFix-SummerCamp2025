/* eslint-disable @typescript-eslint/no-explicit-any */
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
export class Table<T extends Record<string, any> = Record<string, any>> {
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

  getValue(item: T, key: string): any {
    return item[key]
  }

  getKeys(item: T): string[] {
    return Object.keys(item)
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
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
}
