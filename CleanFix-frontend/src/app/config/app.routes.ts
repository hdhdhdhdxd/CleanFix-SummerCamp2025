import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: 'home',
    loadChildren: () =>
      import('@/ui/pages/home/home.routes').then((r) => r.HOME_ROUTES),
  },
  {
    path: 'service-request',
    loadChildren: () =>
      import('@/ui/pages/service-request/service-request.routes').then(
        (r) => r.SERVICE_REQUEST_ROUTES
      ),
  },
  {
    path: '**',
    redirectTo: 'home',
  },
];
