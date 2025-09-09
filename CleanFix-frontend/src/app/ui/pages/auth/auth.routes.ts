import { Routes } from '@angular/router'
import { Login } from './components/login/login'
import { Auth } from './auth'

export const AUTH_ROUTES: Routes = [
  {
    path: '',
    component: Auth,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: Login },
      {
        path: '**',
        redirectTo: 'login',
      },
    ],
  },
]
