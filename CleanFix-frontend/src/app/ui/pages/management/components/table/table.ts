import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { CurrencyPipe, DatePipe } from '@angular/common'
import { Component, inject, OnInit, signal } from '@angular/core'

@Component({
  selector: 'app-table',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './table.html',
})
export class Table implements OnInit {
  solicitations = signal<Solicitation[]>([])

  solicitationService: SolicitationService = inject(SolicitationService)

  ngOnInit() {
    this.loadSolicitations()
  }

  private async loadSolicitations() {
    const solicitations = await this.solicitationService.getAll()
    this.solicitations.set(solicitations)
  }
}
