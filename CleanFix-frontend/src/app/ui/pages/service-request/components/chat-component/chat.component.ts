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
  showDownloadButton = signal(false)
  facturaData = signal<{ empresaNombre: string; materialesNombres: string[] } | null>(null)
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
    // Reset botón de descarga y datos cada vez que se envía un mensaje
    this.showDownloadButton.set(false)
    this.facturaData.set(null)
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

    this.chatService.sendMessage(userMessage, historial).subscribe(
      (res) => {
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

        // Detectar si el mensaje del bot incluye la opción de descargar factura
        if (
          botText.toLowerCase().includes('descargar factura') ||
          botText.toLowerCase().includes('descargar la factura') ||
          botText.toLowerCase().includes('aquí tienes el resumen de la factura')
        ) {
          // Extraer datos de la factura del último mensaje del usuario
          const empresaNombre = this.extractEmpresaNombre()
          const materialesNombres = this.extractMaterialesNombres()
          this.facturaData.set({ empresaNombre, materialesNombres })
          this.showDownloadButton.set(true)
        }
      },
      () => {
        // Reemplazar el último mensaje 'Escribiendo...' por el error
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs.slice(0, -1),
          { from: 'bot', text: 'Ocurrió un error al procesar tu mensaje.' },
        ])
        this.scrollToBottom()
      },
    )
  }

  private extractEmpresaNombre(): string {
    const lastUserMsg =
      this.messages()
        .filter((m) => m.from === 'user')
        .slice(-1)[0]?.text || ''
    const match = lastUserMsg.match(/empresa\s+([\w\d]+)/i)
    return match ? match[1] : ''
  }

  private extractMaterialesNombres(): string[] {
    const lastUserMsg =
      this.messages()
        .filter((m) => m.from === 'user')
        .slice(-1)[0]?.text || ''
    const matches = [...lastUserMsg.matchAll(/material\s+([\w\d]+)/gi)]
    return matches.map((m) => m[1])
  }

  goHome() {
    this.router.navigate(['/'])
  }

  descargarFactura() {
    const data = this.facturaData()
    if (!data) return
    fetch('https://localhost:7096/api/chatboxia/factura/pdf', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    })
      .then(async (response) => {
        if (!response.ok) throw new Error('Error al descargar la factura')
        const blob = await response.blob()
        const url = window.URL.createObjectURL(blob)
        const a = document.createElement('a')
        a.href = url
        a.download = 'factura.pdf'
        document.body.appendChild(a)
        a.click()
        a.remove()
        window.URL.revokeObjectURL(url)
      })
      .catch(() => {
        alert('No se pudo descargar la factura')
      })
  }
}
