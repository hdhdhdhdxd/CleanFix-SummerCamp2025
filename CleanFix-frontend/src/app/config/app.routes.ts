import { Login } from '@/ui/pages/login/login'
import { Routes } from '@angular/router'

export const routes: Routes = [
  { path: '', redirectTo: '', pathMatch: 'full' },
  {
    path: '',
    loadChildren: () => import('@/ui/pages/home/home.routes').then((r) => r.HOME_ROUTES),
  },
  {
    path: 'service-request',
    loadChildren: () =>
      import('@/ui/pages/service-request/service-request.routes').then(
        (r) => r.SERVICE_REQUEST_ROUTES,
      ),
  },
  {
    path: 'management',
    loadChildren: () =>
      import('@/ui/pages/management/management.routes').then((r) => r.MANAGEMENT_ROUTES),
  },
  { path: 'login', component: Login },
  {
    path: '**',
    redirectTo: '',
  },
]
