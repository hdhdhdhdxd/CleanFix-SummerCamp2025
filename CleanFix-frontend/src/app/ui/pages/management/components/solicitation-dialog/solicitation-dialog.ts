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
  computed,
} from '@angular/core'
import { MaterialService } from '@/ui/services/material/material-service'
import { Material } from '@/core/domain/models/Material'

@Component({
  selector: 'app-solicitation-dialog',
  imports: [CommonModule, DatePipe],
  templateUrl: './solicitation-dialog.html',
  styleUrls: ['./solicitation-dialog.css'],
})
export class SolicitationDialog implements AfterViewInit {
  private materialService = inject(MaterialService)

  selectedSolicitation = input.required<Solicitation>()
  companies = input.required<Company[]>()
  selectedCompanyId = signal<number | null>(null)
  materials = signal<Material[]>([])
  apartmentCount = signal<number | null>(null)

  get selectedCompany() {
    const id = this.selectedCompanyId()
    return this.companies().find((c) => c.id === id) || null
  }

  onCompanyChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value
    this.selectedCompanyId.set(value ? +value : null)
    this.loadRandomMaterials()
    this.loadApartmentCount()
  }

  loadRandomMaterials() {
    const issueTypeId = this.selectedSolicitation().issueType.id
    this.materialService.getRandomThree(issueTypeId).subscribe((materials) => {
      this.materials.set(materials)
    })
  }
  closeDialog = output<void>()

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

  loadApartmentCount() {
    this.apartmentCount.set(30)
  }

  getTotalMaterialCost = computed(() => {
    const count = this.apartmentCount() || 0
    const maintenance = this.selectedSolicitation().maintenanceCost || 0
    return this.materials().reduce((acc, m) => acc + m.cost * count * maintenance, 0)
  })

  get totalUnitMaterialCost(): number {
    return this.materials().reduce((acc, m) => acc + m.cost, 0)
  }
}
