import { Router } from '@angular/router'
// chat.component.ts
import { CommonModule } from '@angular/common'
import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { marked } from 'marked'
import { ChatService } from 'src/app/ui/services/chat/chat.service'

@Component({
  selector: 'app-chat',
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent {
  // Eliminar el efecto signal para evitar scroll al escribir
  @ViewChild('chatScroll', { static: false }) chatScroll!: ElementRef<HTMLDivElement>
  // ...existing code...

  private scrollToBottom() {
    if (this.chatScroll && this.chatScroll.nativeElement) {
      setTimeout(() => {
        this.chatScroll.nativeElement.scrollTop = this.chatScroll.nativeElement.scrollHeight
      }, 0)
    }
  }
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
      { from: 'bot', text: 'Escribiendo...' },
    ])
    this.scrollToBottom()
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
        // Convertir Markdown a HTML solo para mensajes del bot
        const botHtml = String(marked.parse(botText))
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs.slice(0, -1),
          { from: 'bot', text: botHtml },
        ])
        this.scrollToBottom()
      },
      error: () => {
        // Reemplazar el último mensaje 'Escribiendo...' por el error
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs.slice(0, -1),
          { from: 'bot', text: 'Ocurrió un error al procesar tu mensaje.' },
        ])
        this.scrollToBottom()
      },
    })
  }

  goHome() {
    this.router.navigate(['/'])
  }
}
