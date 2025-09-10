// Tipo para la respuesta del bot que puede incluir pdfUrl
interface BotResponse {
  mensaje: string
  pdfUrl?: string
  historial?: string[]
}

import { Router } from '@angular/router'
// chat.component.ts
import { CommonModule } from '@angular/common'
import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { marked } from 'marked'
import { ChatService } from 'src/app/ui/services/chat/chat.service'
import { environment } from 'src/environments/environment'

@Component({
  selector: 'app-chat',
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent {
  // Devuelve true si el mensaje en el índice i es el último mensaje del bot
  isLastBotMessage(index: number): boolean {
    const msgs = this.messages()
    for (let i = msgs.length - 1; i >= 0; i--) {
      if (msgs[i].from === 'bot') {
        return i === index
      }
    }
    return false
  }
  showDownloadButton = signal(false)
  facturaData = signal<{ empresaNombre: string; materialesNombres: string[] } | null>(null)
  pdfUrl: string | null = null
  mostrarEmailInput = false
  emailDestino = ''
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
          typeof (res.data as BotResponse).mensaje === 'string'
        ) {
          const botRes = res.data as BotResponse
          botText = botRes.mensaje
          if (botRes.pdfUrl) {
            this.pdfUrl = botRes.pdfUrl
          }
        }
        // Convertir Markdown a HTML solo para mensajes del bot
        const botHtml = String(marked.parse(botText))
        this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
          ...msgs.slice(0, -1),
          { from: 'bot', text: botHtml },
        ])
        this.scrollToBottom()

        // Detectar si el mensaje del bot incluye la opción de factura (descargar o enviar por email)
        if (
          /descargar( la)? descargarla|factura|descargar pdf|descargar el pdf|aquí tienes el resumen (de la factura|del pdf)|¿quieres descargarla en formato pdf|prefieres que te la envíe por correo|¿quieres descargarla|¿prefieres que te la envíe por correo/i.test(
            botText,
          )
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
    // Buscar en el último mensaje del bot que contenga "Empresa:"
    const botMsgs = this.messages()
      .filter((m) => m.from === 'bot')
      .map((m) => m.text)
    for (let i = botMsgs.length - 1; i >= 0; i--) {
      const match = botMsgs[i].match(/Empresa:\s*([\w\sáéíóúÁÉÍÓÚüÜñÑ-]+)/i)
      if (match) return match[1].trim()
    }
    return ''
  }

  private extractMaterialesNombres(): string[] {
    // Buscar en el último mensaje del bot que contenga "Materiales:"
    const botMsgs = this.messages()
      .filter((m) => m.from === 'bot')
      .map((m) => m.text)
    for (let i = botMsgs.length - 1; i >= 0; i--) {
      // Extraer la sección de materiales entre "Materiales:" y "IVA"
      const match = botMsgs[i].match(/Materiales:\s*([\s\S]*?)IVA/i)
      if (match) {
        // Separar por " - " y extraer solo el nombre antes del símbolo de euro
        return match[1]
          .split(' - ')
          .map((m) => {
            // Eliminar etiquetas HTML, guiones, espacios y coste
            const nombreMatch = m.match(/([\wáéíóúÁÉÍÓÚüÜñÑ]+)\s*€/)
            return nombreMatch ? nombreMatch[1].trim() : ''
          })
          .filter((m) => m.length > 0)
      }
    }
    return []
  }

  goHome() {
    this.router.navigate(['/'])
  }

  descargarFactura() {
    const data = this.facturaData()
    if (!data || !this.pdfUrl) return
    const pdfUrl = this.pdfUrl.startsWith('http')
      ? this.pdfUrl
      : `https://devdemoapi2.azurewebsites.net/${this.pdfUrl}`
    const jsonBody = JSON.stringify(data)
    console.log('JSON enviado a descargarFactura:', jsonBody)
    fetch(pdfUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: jsonBody,
    })
      .then(async (response) => {
        if (!response.ok) throw new Error('Error al descargar la factura')
        // Si el endpoint devuelve datos de la factura, actualiza el mensaje del bot
        const contentType = response.headers.get('Content-Type')
        if (contentType && contentType.includes('application/json')) {
          const facturaInfo = await response.json()
          if (facturaInfo && facturaInfo.mensaje) {
            const botHtml = String(marked.parse(facturaInfo.mensaje))
            this.messages.update((msgs: { from: 'user' | 'bot'; text: string }[]) => [
              ...msgs,
              { from: 'bot', text: botHtml },
            ])
            this.scrollToBottom()
          }
        } else {
          // Si es PDF, descarga el archivo
          const blob = await response.blob()
          const url = window.URL.createObjectURL(blob)
          const a = document.createElement('a')
          a.href = url
          a.download = 'factura.pdf'
          document.body.appendChild(a)
          a.click()
          a.remove()
          window.URL.revokeObjectURL(url)
        }
      })
      .catch(() => {
        alert('No se pudo descargar la factura')
      })
  }

  enviarFacturaPorEmail() {
    const data = this.facturaData()
    if (!data || !this.emailDestino) {
      alert('Debes introducir un email válido')
      return
    }
    fetch(`${environment.baseUrl}chatboxia/factura/gmail`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        empresaNombre: data.empresaNombre,
        materialesNombres: data.materialesNombres,
        emailDestino: this.emailDestino,
      }),
    })
      .then(async (response) => {
        if (!response.ok) throw new Error('Error al enviar la factura por email')
        alert('Factura enviada correctamente')
        this.mostrarEmailInput = false
        this.emailDestino = ''
      })
      .catch(() => {
        alert('No se pudo enviar la factura por email')
      })
  }
}
