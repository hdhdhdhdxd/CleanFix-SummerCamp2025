import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { Incidence } from '@/core/domain/models/Incedence'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'
import { CurrencyPipe, DatePipe, NgClass, NgIf } from '@angular/common'
import { Component, input, output } from '@angular/core'

export type TableColumn<T> =
  | {
      key: keyof T
      label: string
      type: 'text' | 'currency' | 'date' | 'number' | 'status'
    }
  | TableColumnProgress<T>
  | TableColumnBoolean<T>

export interface TableColumnProgress<T> {
  key: keyof T
  label: string
  type: 'progress'
  keyStart: keyof T
  keyEnd: keyof T
}

export interface TableColumnBoolean<T> {
  key: keyof T
  label: string
  type: 'boolean'
  labelTrue: string
  labelFalse: string
}

@Component({
  selector: 'app-table',
  imports: [CurrencyPipe, DatePipe, NgClass, NgIf],
  templateUrl: './table.html',
})
export class Table<T extends SolicitationBrief | Incidence | CompletedTaskBrief> {
  items = input<T[]>([])
  tableColumns = input.required<TableColumn<T>[]>()
  rowClick = output<T>()

  handleRowClick(item: T): void {
    this.rowClick.emit(item)
  }

  getColumns(): TableColumn<T>[] {
    const customColumns = this.tableColumns()
    if (!customColumns || !(customColumns.length > 0)) {
      return []
    }
    return customColumns
  }

  getProgressValues(item: T, column: TableColumnProgress<T>): { inicio: Date; fin: Date } | null {
    return {
      inicio: item[column.keyStart] as Date,
      fin: item[column.keyEnd] as Date,
    }
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

  getProgressBarInfoRango(
    fechaInicio: Date,
    fechaFin: Date,
  ): {
    percent: number
    status: 'completed' | 'in_progress'
  } {
    if (!fechaInicio || !fechaFin) {
      return { percent: 0, status: 'in_progress' }
    }
    const now = new Date()
    const start = fechaInicio
    const end = fechaFin
    if (now >= end) {
      return { percent: 100, status: 'completed' }
    }
    if (isNaN(start.getTime()) || isNaN(end.getTime()) || start >= end) {
      return { percent: 0, status: 'in_progress' }
    }
    if (now <= start) {
      return { percent: 0, status: 'in_progress' }
    }
    const total = end.getTime() - start.getTime()
    const elapsed = now.getTime() - start.getTime()
    const percent = Math.max(0, Math.min(100, Math.round((elapsed / total) * 100)))
    return { percent, status: 'in_progress' }
  }
}
