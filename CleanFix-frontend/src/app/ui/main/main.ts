import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer],
  templateUrl: './main.html',
})
export class Main {}
