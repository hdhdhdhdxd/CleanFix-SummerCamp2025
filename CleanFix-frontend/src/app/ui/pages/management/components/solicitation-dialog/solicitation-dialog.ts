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
} from '@angular/core'

@Component({
  selector: 'app-solicitation-dialog',
  imports: [CommonModule, DatePipe],
  templateUrl: './solicitation-dialog.html',
  styleUrls: ['./solicitation-dialog.css'],
})
export class SolicitationDialog implements AfterViewInit {
  selectedSolicitation = input.required<Solicitation>()
  companies = input.required<Company[]>()
  selectedCompanyId = signal<number | null>(null)

  get selectedCompany() {
    const id = this.selectedCompanyId()
    return this.companies().find((c) => c.id === id) || null
  }

  onCompanyChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value
    this.selectedCompanyId.set(value ? +value : null)
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
}
