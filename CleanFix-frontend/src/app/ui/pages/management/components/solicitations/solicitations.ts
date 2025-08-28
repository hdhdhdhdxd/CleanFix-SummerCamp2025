import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { Component, inject, OnInit, signal } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { Pagination } from '../pagination/pagination'
import { SearchBar } from '../search-bar/search-bar'
import { SolicitationDialog } from '../solicitation-dialog/solicitation-dialog'
import { Table, TableColumn } from '../table/table'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { Company } from '@/core/domain/models/Company'
import { CompanyService } from '@/ui/services/company/company-service'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table, SolicitationDialog, Pagination],
  templateUrl: './solicitations.html',
})
export class Solicitations implements OnInit {
  private route = inject(ActivatedRoute)
  private location = inject(Location)
  private solicitationService = inject(SolicitationService)
  private companyService = inject(CompanyService)

  solicitations = signal<SolicitationBrief[]>([])
  totalPages = signal<number>(0)
  totalCount = signal<number>(0)
  hasPreviousPage = signal<boolean>(false)
  hasNextPage = signal<boolean>(false)

  pageSize = signal<number>(10)
  pageNumber = signal<number>(1)

  selectedSolicitation: Solicitation | null = null
  companies = signal<Company[]>([])
  showDialog = signal<boolean>(false)

  columns: TableColumn<SolicitationBrief>[] = [
    { key: 'address', label: 'Dirección', type: 'text' },
    { key: 'date', label: 'Fecha', type: 'date' },
    { key: 'issueType', label: 'Tipo', type: 'text' },
  ]

  ngOnInit() {
    const currentParams = this.route.snapshot.queryParams
    const initialPageSize = +(currentParams['pageSize'] || 10)
    const initialPageNumber = +(currentParams['pageNumber'] || 1)

    this.pageSize.set(initialPageSize)
    this.pageNumber.set(initialPageNumber)

    if (!currentParams['pageSize'] && !currentParams['pageNumber']) {
      this.updateUrl()
    }
    this.loadSolicitations(this.pageNumber(), this.pageSize())
  }

  private updateUrl(): void {
    const queryParams = new URLSearchParams()
    queryParams.set('pageSize', this.pageSize().toString())
    queryParams.set('pageNumber', this.pageNumber().toString())

    const url = `${this.location.path().split('?')[0]}?${queryParams.toString()}`
    this.location.replaceState(url)
  }

  private loadSolicitations(page: number, pageSize: number) {
    this.solicitationService.getPaginated(page, pageSize).subscribe((result) => {
      this.updateValues(result)
    })
  }

  private updateValues(pagination: PaginatedData<SolicitationBrief>) {
    this.solicitations.set(pagination.items)
    this.pageNumber.set(pagination.pageNumber)
    this.totalPages.set(pagination.totalPages)
    this.totalCount.set(pagination.totalCount)
    this.hasPreviousPage.set(pagination.hasPreviousPage)
    this.hasNextPage.set(pagination.hasNextPage)
  }

  setPageNumber($event: number) {
    this.pageNumber.set($event)
    this.updateUrl()
    this.loadSolicitations($event, this.pageSize())
  }

  setPageSize($event: number) {
    this.pageSize.set($event)
    this.pageNumber.set(1)
    this.updateUrl()
    this.loadSolicitations(1, $event)
  }

  handleRowClick(item: SolicitationBrief): void {
    this.getSolicitationById(item.id)
  }

  handleCloseDialog(): void {
    this.selectedSolicitation = null
    this.showDialog.set(false)
  }

  private getSolicitationById(id: number): void {
    this.solicitationService.getById(id).subscribe((solicitation) => {
      this.onSolicitationLoaded(solicitation)
      this.loadCompanies()
    })
  }

  private loadCompanies(): void {
    this.companyService
      .getPaginated(1, 10, this.selectedSolicitation?.issueType.id)
      .subscribe((companies) => {
        this.companies.set(companies.items)
      })
  }

  private onSolicitationLoaded(solicitation: Solicitation): void {
    this.selectedSolicitation = solicitation
    this.showDialog.set(true)
  }
}
