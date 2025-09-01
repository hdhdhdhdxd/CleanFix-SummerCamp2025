import { Router } from '@angular/router'
// chat.component.ts
import { CommonModule } from '@angular/common'
import { Component, inject, signal } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { ChatService } from 'src/app/ui/services/chat/chat.service'

@Component({
  selector: 'app-chat',
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent {
  messages = signal<{ from: 'user' | 'bot'; text: string }[]>([])
  newMessage = ''

  chatService = inject(ChatService)
  router = inject(Router)

  sendMessage() {
    const userMessage = this.newMessage.trim()
    if (!userMessage) return

    // Construir historial solo con los textos enviados por el usuario
    const historial = this.messages()
      .filter((m) => m.from === 'user')
      .map((m) => m.text)

    this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
      ...msgs,
      { from: 'user', text: userMessage },
    ])
    this.newMessage = ''

    this.chatService.sendMessage(userMessage, historial).subscribe({
      next: (res) => {
        let botText = ''
        if (typeof res.data === 'string') {
          botText = res.data
        } else if (
          res.data &&
          typeof res.data === 'object' &&
          typeof res.data.mensaje === 'string'
        ) {
          botText = res.data.mensaje
        }
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs,
          { from: 'bot', text: botText },
        ])
      },
      error: () => {
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs,
          { from: 'bot', text: 'Ocurrió un error al procesar tu mensaje.' },
        ])
      },
    })
  }

  goHome() {
    this.router.navigate(['/'])
  }
}
