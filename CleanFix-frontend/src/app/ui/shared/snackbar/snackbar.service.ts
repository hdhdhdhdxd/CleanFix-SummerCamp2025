import { Injectable, signal } from '@angular/core'

@Injectable({ providedIn: 'root' })
export class SnackbarService {
  message = signal<string>('')
  isSuccess = signal<boolean>(true)
  visible = signal<boolean>(false)

  private timeoutId: number | undefined

  show(message: string, isSuccess = true, duration = 3000) {
    this.message.set(message)
    this.isSuccess.set(isSuccess)
    this.visible.set(true)
    clearTimeout(this.timeoutId)
    this.timeoutId = setTimeout(() => this.hide(), duration)
  }

  hide() {
    this.visible.set(false)
  }
}
