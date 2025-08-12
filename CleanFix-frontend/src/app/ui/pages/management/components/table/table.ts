import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common'
import { Component, inject, OnInit, signal, computed } from '@angular/core'

@Component({
  selector: 'app-table',
  imports: [CurrencyPipe, DatePipe, NgClass],
  templateUrl: './table.html',
})
export class Table implements OnInit {
  solicitations = signal<Solicitation[]>([])
  currentPage = signal(1)
  itemsPerPage = signal(5)

  solicitationService: SolicitationService = inject(SolicitationService)

  // Computed properties para el paginado
  totalItems = computed(() => this.solicitations().length)
  totalPages = computed(() => Math.ceil(this.totalItems() / this.itemsPerPage()))

  paginatedSolicitations = computed(() => {
    const start = (this.currentPage() - 1) * this.itemsPerPage()
    const end = start + this.itemsPerPage()
    return this.solicitations().slice(start, end)
  })

  ngOnInit() {
    this.loadSolicitations()
  }

  private async loadSolicitations() {
    const solicitations = await this.solicitationService.getAll()
    this.solicitations.set(solicitations)
  }

  // Métodos de paginación
  previousPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update((page) => page - 1)
    }
  }

  nextPage() {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((page) => page + 1)
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages()) {
      this.currentPage.set(page)
    }
  }

  getPageNumbers(): number[] {
    const total = this.totalPages()
    const current = this.currentPage()
    const pages: number[] = []

    // Mostrar máximo 5 páginas
    const start = Math.max(1, current - 2)
    const end = Math.min(total, start + 4)

    for (let i = start; i <= end; i++) {
      pages.push(i)
    }

    return pages
  }

  // Método para estilos de estado
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
