import { Component } from '@angular/core'
import { ChatboxComponent } from '../chatbox/chatbox'
import { ChatComponent } from './components/chat-component/chat.component'

@Component({
  selector: 'app-service-request',
  imports: [ChatboxComponent, ChatComponent],
  templateUrl: './service-request.html',
})
export class ServiceRequest {}
