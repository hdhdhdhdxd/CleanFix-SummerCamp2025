import { CommonModule } from '@angular/common'
import { Component, inject, signal, effect } from '@angular/core'
import { SnackbarService } from './snackbar.service'

@Component({
  selector: 'app-snackbar',
  imports: [CommonModule],
  templateUrl: './snackbar.html',
  styleUrls: ['./snackbar.css'],
})
export class Snackbar {
  snackbar = inject(SnackbarService)

  message = this.snackbar.message
  isSuccess = this.snackbar.isSuccess

  showing = signal(false)
  leaving = signal(false)
  entering = signal(false)

  constructor() {
    effect(() => {
      if (this.snackbar.visible()) {
        this.showing.set(true)
        this.leaving.set(false)
        this.entering.set(true)
        setTimeout(() => {
          this.entering.set(false)
        }, 400)
      } else if (this.showing()) {
        this.leaving.set(true)
        setTimeout(() => {
          this.showing.set(false)
        }, 400)
      }
    })
  }

  close() {
    this.snackbar.hide()
  }
}
