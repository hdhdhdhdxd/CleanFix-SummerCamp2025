import { Component } from '@angular/core'
import { CardInfo } from './components/card-info/card-info'
import { Hero } from './components/hero/hero'

@Component({
  selector: 'app-home',
  imports: [Hero, CardInfo],
  templateUrl: './home.html',
})
export class Home {}
