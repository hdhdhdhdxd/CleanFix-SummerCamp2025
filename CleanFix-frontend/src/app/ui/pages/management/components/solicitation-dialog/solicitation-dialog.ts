import { Solicitation } from '@/core/domain/models/Solicitation'
import { CurrencyPipe, DatePipe } from '@angular/common'
import { AfterViewInit, Component, ElementRef, input, output, ViewChild } from '@angular/core'

@Component({
  selector: 'app-solicitation-dialog',
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './solicitation-dialog.html',
  styles: [
    `
      dialog {
        transition:
          transform 0.4s cubic-bezier(0.16, 1, 0.3, 1),
          opacity 0.3s cubic-bezier(0.16, 1, 0.3, 1),
          backdrop-filter 0.3s cubic-bezier(0.16, 1, 0.3, 1),
          display 0.4s allow-discrete;

        transform: scale(0.7) translateY(50px);
        opacity: 0;
      }

      dialog[open] {
        transform: scale(1) translateY(0);
        opacity: 1;

        @starting-style {
          transform: scale(0.7) translateY(50px);
          opacity: 0;
        }
      }

      dialog::backdrop {
        transition:
          backdrop-filter 0.3s cubic-bezier(0.16, 1, 0.3, 1),
          background-color 0.3s cubic-bezier(0.16, 1, 0.3, 1);

        backdrop-filter: blur(8px);
        background-color: rgba(0, 0, 0, 0.4);
      }

      dialog[open]::backdrop {
        @starting-style {
          backdrop-filter: blur(0px);
          background-color: rgba(0, 0, 0, 0);
        }
      }

      .dialog-content {
        transition:
          transform 0.5s cubic-bezier(0.16, 1, 0.3, 1),
          opacity 0.4s cubic-bezier(0.16, 1, 0.3, 1);
        animation: slideInUp 0.5s cubic-bezier(0.16, 1, 0.3, 1);
      }

      @keyframes slideInUp {
        0% {
          transform: translateY(100px) scale(0.9);
          opacity: 0;
        }
        100% {
          transform: translateY(0) scale(1);
          opacity: 1;
        }
      }

      @keyframes slideOutDown {
        0% {
          transform: translateY(0) scale(1);
          opacity: 1;
        }
        100% {
          transform: translateY(50px) scale(0.95);
          opacity: 0;
        }
      }

      .dialog-content.closing {
        animation: slideOutDown 0.3s cubic-bezier(0.4, 0, 1, 1) forwards;
      }
    `,
  ],
})
export class SolicitationDialog implements AfterViewInit {
  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>
  @ViewChild('dialogContent') dialogContent!: ElementRef<HTMLElement>

  selectedSolicitation = input.required<Solicitation>()
  closeDialog = output<void>()

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
