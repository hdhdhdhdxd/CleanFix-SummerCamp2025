import { adminGuard } from '@/ui/guards/admin/admin-guard'
import { authGuard } from '@/ui/guards/auth/auth-guard'
import { guestGuard } from '@/ui/guards/guest/guest-guard'
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
    canActivate: [authGuard],
  },
  {
    path: 'management',
    loadChildren: () =>
      import('@/ui/pages/management/management.routes').then((r) => r.MANAGEMENT_ROUTES),
    canActivate: [adminGuard],
  },
  {
    path: 'auth',
    loadChildren: () => import('@/ui/pages/auth/auth.routes').then((r) => r.AUTH_ROUTES),
    canActivate: [guestGuard],
  },
  {
    path: '**',
    redirectTo: '',
  },
]
