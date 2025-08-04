import { Component } from '@angular/core'
import { StatCard } from '../stat-card/stat-card'

@Component({
  selector: 'app-stats-section',
  imports: [StatCard],
  templateUrl: './stats-section.html',
})
export class StatsSection {}
