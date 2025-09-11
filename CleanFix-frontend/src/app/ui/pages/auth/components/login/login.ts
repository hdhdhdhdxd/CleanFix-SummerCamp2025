import { UserService } from '@/ui/services/user/user-service'
import { AuthService } from '@/ui/services/auth/auth-service'
import { CommonModule } from '@angular/common'
import { Component, inject } from '@angular/core'
import { switchMap } from 'rxjs/operators'
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms'
import { Router, RouterLink } from '@angular/router'

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  private readonly formBuilder = inject(FormBuilder)
  private readonly userService = inject(UserService)
  private readonly authService = inject(AuthService)
  private readonly router = inject(Router)

  loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    rememberMe: [false, [Validators.required]],
  })

  onSubmit() {
    if (this.loginForm.invalid) return

    const { email, password, rememberMe } = this.loginForm.value

    this.userService
      .login(email!, password!, rememberMe!)
      .pipe(switchMap(() => this.authService.loadUser()))
      .subscribe({
        next: () => {
          this.router.navigate(['/'])
        },
        error: (err) => {
          console.error('Login failed or error loading user', err)
        },
      })
  }
}
