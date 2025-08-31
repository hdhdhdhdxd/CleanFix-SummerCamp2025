import { Incidence } from '@/core/domain/models/Incidence'
import { Company } from '@/core/domain/models/Company'
import { Material } from '@/core/domain/models/Material'
import { IncidenceService } from '@/ui/services/incidence/incidence-service'
import { MaterialService } from '@/ui/services/material/material-service'
import { CompanyService } from '@/ui/services/company/company-service'
import { CompletedTaskService } from '@/ui/services/completedtask/completed-task-service'
import { SnackbarService } from '@/ui/shared/snackbar/snackbar.service'
import { DatePipe, CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  input,
  OnInit,
  output,
  signal,
  ViewChild,
} from '@angular/core'

@Component({
  selector: 'app-incidence-dialog',
  imports: [CommonModule, DatePipe, FormsModule],
  templateUrl: './incidence-dialog.html',
})
export class IncidenceDialog implements OnInit, AfterViewInit {
  private incidenceService = inject(IncidenceService)
  private materialService = inject(MaterialService)
  private companyService = inject(CompanyService)
  private completedTaskService = inject(CompletedTaskService)
  private snackbarService = inject(SnackbarService)

  incidenceId = input.required<number>()
  closeDialog = output<void>()
  taskCreated = output<void>() // Nuevo evento para cuando se crea una tarea

  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>
  @ViewChild('dialogContent') dialogContent!: ElementRef<HTMLElement>

  incidence = signal<Incidence | null>(null)
  companies = signal<Company[]>([])
  selectedCompanyId = signal<number | null>(null)
  materials = signal<Material[]>([])

  ngOnInit(): void {
    this.fetchIncidenceAndCompanies()
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

  private fetchIncidenceAndCompanies(): void {
    this.incidenceService.getById(this.incidenceId()).subscribe((incidence) => {
      this.incidence.set(incidence)
      this.fetchCompanies(incidence.issueType.id)
    })
  }

  private fetchCompanies(issueTypeId: number): void {
    this.companyService.getPaginated(1, 10, issueTypeId).subscribe((companies) => {
      this.companies.set(companies.items)
    })
  }

  private fetchRandomMaterials(): void {
    const incidence = this.incidence()
    if (!incidence) return
    const issueTypeId = incidence.issueType.id
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

  get surfaceValue(): number {
    return this.incidence()?.surface ?? 0
  }

  get totalMaterialsCost(): number {
    const surface = this.surfaceValue
    const incidence = this.incidence()
    if (!incidence) return 0

    return this.materials().reduce((acc, m) => {
      return acc + m.costPerSquareMeter * surface
    }, 0)
  }

  get totalCompanyCost(): number {
    const company = this.selectedCompany
    if (!company) return 0
    // Usar el precio real de la empresa
    return company.price
  }

  get totalJobCost(): number {
    return this.totalMaterialsCost + this.totalCompanyCost
  }

  createTask() {
    const incidence = this.incidence()
    const company = this.selectedCompany
    const materials = this.materials()
    if (!incidence || !company || !materials.length || company.id === 0) {
      throw new Error('Invalid data')
    }
    this.completedTaskService
      .create(
        0,
        incidence.id,
        company.id,
        false,
        materials.map((m) => m.id),
      )
      .subscribe({
        next: () => {
          this.snackbarService.show('Tarea creada con éxito', true)
          this.taskCreated.emit() // Emitir evento de tarea creada
          this.closeModal()
        },
        error: (err) => {
          this.snackbarService.show('Error al crear tarea', false)
          throw err
        },
      })
  }

  private fetchIncidence(): void {
    this.incidenceService.getById(this.incidenceId()).subscribe((incidence) => {
      this.incidence.set(incidence)
    })
  }
}
