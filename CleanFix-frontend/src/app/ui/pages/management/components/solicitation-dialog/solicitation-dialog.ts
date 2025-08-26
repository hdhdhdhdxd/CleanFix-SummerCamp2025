import { Company } from '@/core/domain/models/Company'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { CompanyService } from '@/ui/services/company/company-service'
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common'
import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  input,
  output,
  signal,
  ViewChild,
} from '@angular/core'

@Component({
  selector: 'app-solicitation-dialog',
  imports: [CommonModule, CurrencyPipe, DatePipe],
  templateUrl: './solicitation-dialog.html',
  styleUrls: ['./solicitation-dialog.css'],
})
export class SolicitationDialog implements AfterViewInit {
  private readonly companyService = inject(CompanyService)

  companies = signal<Company[]>([])
  private _loadingCompanies = signal<boolean>(false)
  private _companiesLoaded = false

  selectedSolicitation = input.required<Solicitation>()
  closeDialog = output<void>()

  get loadingCompanies() {
    return this._loadingCompanies()
  }

  loadCompanies(): void {
    if (this._companiesLoaded || this._loadingCompanies()) {
      return
    }
    this._loadingCompanies.set(true)
    this.companyService.getAll().subscribe({
      next: (companies) => {
        this.companies.set(companies)
        this._loadingCompanies.set(false)
        this._companiesLoaded = true
      },
      error: () => {
        this._loadingCompanies.set(false)
      },
    })
  }

  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>
  @ViewChild('dialogContent') dialogContent!: ElementRef<HTMLElement>

  ngAfterViewInit(): void {
    this.openDialog()
  }

  openDialog(): void {
    this.dialog.nativeElement.showModal()
  }

  closeModal(): void {
    this.dialogContent.nativeElement.classList.add('closing')

    this.dialog.nativeElement.style.transform = 'scale(0.95) translateY(20px)'
    this.dialog.nativeElement.style.opacity = '0'

    setTimeout(() => {
      this.dialog.nativeElement.close()
      this.closeDialog.emit()
    }, 300)
  }

  handleBackdropClick(event: MouseEvent): void {
    if (event.target === this.dialog.nativeElement) {
      this.closeModal()
    }
  }
}
