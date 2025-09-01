import { CompletedTask } from '@/core/domain/models/CompletedTask'
import { CompletedTaskService } from '@/ui/services/completedtask/completed-task-service'
import { DatePipe, CurrencyPipe } from '@angular/common'
import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  signal,
  ViewChild,
} from '@angular/core'

@Component({
  selector: 'app-completed-task-dialog',
  imports: [DatePipe, CurrencyPipe],
  templateUrl: './completed-task-dialog.html',
  styleUrls: ['./completed-task-dialog.css'],
})
export class CompletedTaskDialog implements OnInit, AfterViewInit {
  @Input() completedTaskId!: number
  @Output() closeDialog = new EventEmitter<void>()

  private completedTaskService = inject(CompletedTaskService)

  completedTask = signal<CompletedTask | null>(null)
  isLoading = signal<boolean>(true)

  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>
  @ViewChild('dialogContent') dialogContent!: ElementRef<HTMLElement>

  ngOnInit() {
    this.loadCompletedTask()
  }

  ngAfterViewInit(): void {
    this.openDialog()
  }

  private loadCompletedTask() {
    this.isLoading.set(true)
    this.completedTaskService.getById(this.completedTaskId.toString()).subscribe({
      next: (task) => {
        this.completedTask.set(task)
        this.isLoading.set(false)
      },
      error: (error) => {
        console.error('Error loading completed task:', error)
        this.isLoading.set(false)
      },
    })
  }

  openDialog(): void {
    this.dialog.nativeElement.showModal()
  }

  onClose(): void {
    this.dialogContent.nativeElement.classList.add('closing')
    this.dialog.nativeElement.style.transform = 'scale(0.95) translateY(20px)'
    this.dialog.nativeElement.style.opacity = '0'
    setTimeout(() => {
      this.dialog.nativeElement.close()
      this.closeDialog.emit()
    }, 300)
  }

  onBackdropClick(event: MouseEvent): void {
    if (event.target === this.dialog.nativeElement) {
      this.onClose()
    }
  }
}
