import { Home } from '@/ui/pages/home/home'
import { Routes } from '@angular/router'

export const routes: Routes = [
  { path: '', redirectTo: '', pathMatch: 'full' },
  { path: '', component: Home },
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
  {
    path: 'auth',
    loadChildren: () => import('@/ui/pages/auth/auth.routes').then((r) => r.AUTH_ROUTES),
  },
  {
    path: '**',
    redirectTo: '',
  },
]
