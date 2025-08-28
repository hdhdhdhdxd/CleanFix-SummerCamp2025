import { Component } from '@angular/core'
import { FormsModule } from '@angular/forms'

@Component({
  selector: 'app-chatbox',
  imports: [FormsModule],
  template: `
    <div class="chatbox-container">
      <div class="chat-messages" #messages>
        <!-- Aquí se mostrarán los mensajes -->
      </div>
      <form class="chatbox-form" (submit)="sendMessage($event)">
        <input
          type="text"
          [(ngModel)]="userMessage"
          name="message"
          placeholder="Escribe tu mensaje..."
          class="chatbox-input"
          autocomplete="off"
        />
        <button type="submit" class="chatbox-btn">Enviar</button>
      </form>
    </div>
  `,
  styles: [
    `
      .chatbox-container {
        display: flex;
        flex-direction: column;
        height: 400px;
        border-radius: 1rem;
        border: 1px solid #e5e7eb;
        background: #f9fafb;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
        padding: 1rem;
      }
      .chat-messages {
        flex: 1;
        overflow-y: auto;
        margin-bottom: 1rem;
        padding: 0.5rem;
        background: #fff;
        border-radius: 0.5rem;
        border: 1px solid #e5e7eb;
      }
      .chatbox-form {
        display: flex;
        gap: 0.5rem;
      }
      .chatbox-input {
        flex: 1;
        padding: 0.5rem;
        border-radius: 0.5rem;
        border: 1px solid #e5e7eb;
      }
      .chatbox-btn {
        background: #6366f1;
        color: #fff;
        border: none;
        border-radius: 0.5rem;
        padding: 0.5rem 1rem;
        cursor: pointer;
        font-weight: bold;
      }
    `,
  ],
})
export class ChatboxComponent {
  userMessage = ''
  messages: { text: string; from: 'user' | 'ai' }[] = []

  sendMessage(event: Event) {
    event.preventDefault()
    if (!this.userMessage.trim()) return
    this.messages.push({ text: this.userMessage, from: 'user' })
    // Simulación de respuesta IA
    setTimeout(() => {
      this.messages.push({ text: 'Respuesta IA: ' + this.userMessage, from: 'ai' })
    }, 800)
    this.userMessage = ''
  }
}
