import { Routes } from '@angular/router'
import { Management } from './management'

export const MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    component: Management,
    children: [
      { path: '', redirectTo: 'solicitations', pathMatch: 'full' },
      {
        path: 'solicitations',
        loadComponent: () =>
          import('./components/solicitations/solicitations').then((m) => m.Solicitations),
      },
      {
        path: 'requests',
        loadComponent: () => import('./components/requests/requests').then((m) => m.Requests),
      },
      {
        path: 'incidences',
        loadComponent: () => import('./components/incidences/incidences').then((m) => m.Incidences),
      },
    ],
  },
]
