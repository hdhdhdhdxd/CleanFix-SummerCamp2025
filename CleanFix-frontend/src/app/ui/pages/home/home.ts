import { Component } from '@angular/core'
import { Hero } from './components/hero/hero'
import { Brands } from './components/brands/brands'
import { ServiceSection } from './components/service-section/service-section'
import { WhyUsSection } from './components/why-us-section/why-us-section'
import { LocationsSection } from './components/locations-section/locations-section'
import { StatsSection } from './components/stats-section/stats-section'

@Component({
  selector: 'app-home',
  imports: [Hero, Brands, ServiceSection, WhyUsSection, LocationsSection, StatsSection],
  templateUrl: './home.html',
})
export class Home {}
