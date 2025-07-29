import { Component } from '@angular/core'
import { CardInfo } from './components/card-info/card-info'
import { Hero } from './components/hero/hero'
import { Brands } from './components/brands/brands'

@Component({
  selector: 'app-home',
  imports: [Hero, CardInfo, Brands],
  templateUrl: './home.html',
})
export class Home {}
