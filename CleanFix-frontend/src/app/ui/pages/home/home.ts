import { Component } from '@angular/core'
import { CardInfo } from './components/card-info/card-info'
import { Hero } from './components/hero/hero'
import { Brands } from './components/brands/brands'
import { ServiceSection } from './components/service-section/service-section'

@Component({
  selector: 'app-home',
  imports: [Hero, CardInfo, Brands, ServiceSection],
  templateUrl: './home.html',
})
export class Home {}
