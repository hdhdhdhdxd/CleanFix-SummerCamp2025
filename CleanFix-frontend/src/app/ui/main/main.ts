import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'
import { Header } from '../shared/header/header'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Header],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {}
