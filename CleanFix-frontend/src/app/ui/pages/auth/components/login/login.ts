import { UserService } from '@/ui/services/user/user-service'
import { CommonModule } from '@angular/common'
import { Component, inject } from '@angular/core'
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms'
import { RouterLink } from '@angular/router'

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  private readonly formBuilder = inject(FormBuilder)
  private readonly userService = inject(UserService)

  loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    rememberMe: [false, [Validators.required]],
  })

  onSubmit() {
    if (this.loginForm.invalid) return

    const { email, password, rememberMe } = this.loginForm.value

    this.userService.login(email!, password!, rememberMe!).subscribe({
      next: () => {
        console.log('Login successful')
      },
      error: (err) => {
        console.error('Login failed', err)
      },
    })
  }
}
