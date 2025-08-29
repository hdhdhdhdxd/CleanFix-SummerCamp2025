import { Routes } from '@angular/router'
import { Management } from './management'
import { Solicitations } from './components/solicitations/solicitations'
import { Incidences } from './components/incidences/incidences'
import { CompletedTasks } from './components/completed-tasks/completed-tasks'

export const MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    component: Management,
    children: [
      { path: '', redirectTo: 'solicitations', pathMatch: 'full' },
      { path: 'solicitations', component: Solicitations },
      { path: 'completed-tasks', component: CompletedTasks },
      { path: 'incidences', component: Incidences },
      { path: '**', redirectTo: 'solicitations' },
    ],
  },
]
