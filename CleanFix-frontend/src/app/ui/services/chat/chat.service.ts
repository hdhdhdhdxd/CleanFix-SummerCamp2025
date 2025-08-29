import { HttpClient } from '@angular/common/http'
import { Injectable, inject } from '@angular/core'
import { Observable } from 'rxjs'

export interface ChatboxIAResponse {
  success: boolean
  error: string | null
  data: string
}

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private apiUrl = 'https://localhost:7096/api/chatboxia'

  private http = inject(HttpClient)

  sendMessage(message: string): Observable<ChatboxIAResponse> {
    return this.http.post<ChatboxIAResponse>(this.apiUrl, { message })
  }
}
