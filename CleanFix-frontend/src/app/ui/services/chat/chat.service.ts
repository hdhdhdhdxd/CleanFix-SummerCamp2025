import { HttpClient } from '@angular/common/http'
import { Injectable, inject } from '@angular/core'
import { Observable } from 'rxjs'
import { environment } from 'src/environments/environment'

export interface ChatboxIAResponse {
  success: boolean
  error: string | null
  data: {
    mensaje: string
    historial?: string[]
  }
}

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private apiUrl = `${environment.baseUrl}/chatboxia`

  private http = inject(HttpClient)

  sendMessage(message: string, historial: string[] = []): Observable<ChatboxIAResponse> {
    return this.http.post<ChatboxIAResponse>(this.apiUrl, {
      mensaje: message,
      historial: historial,
    })
  }
}
