import { Company } from '@/core/domain/models/Company'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { DatePipe, CommonModule } from '@angular/common'
import {
  AfterViewInit,
  Component,
  ElementRef,
  input,
  output,
  signal,
  ViewChild,
  inject,
  OnInit,
} from '@angular/core'
import { MaterialService } from '@/ui/services/material/material-service'
import { Material } from '@/core/domain/models/Material'
import { SolicitationService } from '@/ui/services/solicitation/solicitation-service'
import { CompanyService } from '@/ui/services/company/company-service'
import { CompletedTaskService } from '@/ui/services/completedtask/completed-task-service'
import { FormsModule } from '@angular/forms'
import { SnackbarService } from '@/ui/shared/snackbar/snackbar.service'

@Component({
  selector: 'app-solicitation-dialog',
  imports: [CommonModule, DatePipe, FormsModule],
  templateUrl: './solicitation-dialog.html',
  styleUrls: ['./solicitation-dialog.css'],
})
export class SolicitationDialog implements AfterViewInit, OnInit {
  private solicitationService = inject(SolicitationService)
  private materialService = inject(MaterialService)
  private companyService = inject(CompanyService)
  private completedTaskService = inject(CompletedTaskService)
  private snackbarService = inject(SnackbarService)

  solicitationId = input.required<number>()
  solicitation = signal<Solicitation | null>(null)
  companies = signal<Company[]>([])
  selectedCompanyId = signal<number | null>(null)
  materials = signal<Material[]>([])

  closeDialog = output<void>()
  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>
  @ViewChild('dialogContent') dialogContent!: ElementRef<HTMLElement>

  ngOnInit(): void {
    this.fetchSolicitationAndCompanies()
  }

  ngAfterViewInit(): void {
    this.openDialog()
  }

  get selectedCompany(): Company | null {
    const id = this.selectedCompanyId()
    return this.companies().find((c) => c.id === id) || null
  }

  onCompanyChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value
    this.selectedCompanyId.set(value ? +value : null)
    this.fetchRandomMaterials()
  }

  private fetchSolicitationAndCompanies(): void {
    this.solicitationService.getById(this.solicitationId()).subscribe((solicitation) => {
      this.solicitation.set(solicitation)
      this.fetchCompanies(solicitation.issueType.id)
    })
  }

  private fetchCompanies(issueTypeId: number): void {
    this.companyService.getPaginated(1, 10, issueTypeId).subscribe((companies) => {
      this.companies.set(companies.items)
    })
  }

  private fetchRandomMaterials(): void {
    const solicitation = this.solicitation()
    if (!solicitation) return
    const issueTypeId = solicitation.issueType.id
    this.materialService.getRandomThree(issueTypeId).subscribe((materials) => {
      this.materials.set(materials)
    })
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

  get apartmentAmountValue(): number {
    return this.solicitation()?.apartmentAmount ?? 0
  }

  get totalMaterialsCost(): number {
    const count = this.apartmentAmountValue
    return this.materials().reduce((acc, m) => acc + m.cost * count, 0)
  }

  get totalCompanyCost(): number {
    const count = this.apartmentAmountValue
    const solicitation = this.solicitation()
    const maintenance = solicitation ? solicitation.maintenanceCost || 0 : 0
    return maintenance * count
  }

  get totalJobCost(): number {
    return this.totalMaterialsCost + this.totalCompanyCost
  }

  createTask() {
    const solicitation = this.solicitation()
    const company = this.selectedCompany
    const materials = this.materials()
    if (!solicitation || !company || !materials.length || company.id === 0) {
      throw new Error('Invalid data')
    }
    this.completedTaskService
      .create(
        solicitation.id,
        solicitation.issueType.id,
        company.id,
        true,
        materials.map((m) => m.id),
      )
      .subscribe({
        next: () => {
          this.snackbarService.show('Tarea creada con éxito', true)
          this.closeModal()
        },
        error: (err) => {
          this.snackbarService.show('Error al crear tarea', false)
          throw err
        },
      })
  }
}
