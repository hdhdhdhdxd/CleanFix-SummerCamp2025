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
      { path: '', redirectTo: 'solicitations', pathMatch: 'full' },
      { path: 'solicitations', component: Solicitations },
      { path: 'requests', component: Requests },
      { path: 'incidences', component: Incidences },
      { path: '**', redirectTo: 'solicitations' },
    ],
  },
]
