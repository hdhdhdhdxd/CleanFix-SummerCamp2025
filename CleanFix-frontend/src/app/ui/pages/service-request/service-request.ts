import { Component } from '@angular/core'
import { ChatComponent } from './components/chat-component/chat.component'

@Component({
  selector: 'app-service-request',
  imports: [ChatComponent],
  templateUrl: './service-request.html',
})
export class ServiceRequest {}
