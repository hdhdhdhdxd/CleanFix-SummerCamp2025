import { Component } from '@angular/core'
import { CardInfo } from './components/card-info/card-info'
import { Hero } from './components/hero/hero'
import { Brands } from './components/brands/brands'
import { ServiceSection } from './components/service-section/service-section'
import { WhyUsSection } from './components/why-us-section/why-us-section'
import { Header } from '@/ui/shared/header/header'

@Component({
  selector: 'app-home',
  imports: [Hero, CardInfo, Brands, ServiceSection, WhyUsSection, Header],
  templateUrl: './home.html',
})
export class Home {}
