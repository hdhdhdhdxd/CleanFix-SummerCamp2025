import { Routes } from '@angular/router'
import { Login } from './components/login/login'

export const AUTH_ROUTES: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  {
    path: '**',
    redirectTo: 'login',
  },
]
