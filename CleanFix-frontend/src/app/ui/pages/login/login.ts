import { Component } from '@angular/core'
import { FormsModule } from '@angular/forms'

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
})
export class Login {
  username = ''
  password = ''

  onSubmit() {
    // Aquí iría la lógica de autenticación
    console.log('Usuario:', this.username)
    console.log('Contraseña:', this.password)
  }
}
