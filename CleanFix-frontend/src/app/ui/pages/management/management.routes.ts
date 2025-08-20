import { Routes } from '@angular/router'
import { Management } from './management'
import { Solicitations } from './components/solicitations/solicitations'
import { Requests } from './components/requests/requests'
import { Incidences } from './components/incidences/incidences'

export const MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    component: Management,
    children: [
      { path: '', redirectTo: 'solicitations/10/1', pathMatch: 'full' },
      {
        path: 'solicitations/:pageSize/:pageNumber',
        component: Solicitations,
      },
      { path: 'requests', component: Requests },
      { path: 'incidences', redirectTo: 'incidences/10/1', pathMatch: 'full' },
      { path: 'incidences/:pageSize/:pageNumber', component: Incidences },
      { path: '**', redirectTo: 'solicitations/10/1' },
    ],
  },
]
