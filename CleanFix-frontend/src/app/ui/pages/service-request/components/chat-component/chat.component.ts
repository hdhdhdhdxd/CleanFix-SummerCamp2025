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

    this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
      ...msgs,
      { from: 'user', text: userMessage },
    ])
    this.newMessage = ''

    this.chatService.sendMessage(userMessage).subscribe({
      next: (res) => {
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs,
          { from: 'bot', text: res.data },
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
