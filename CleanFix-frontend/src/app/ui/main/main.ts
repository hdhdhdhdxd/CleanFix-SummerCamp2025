import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'
import { Header } from '../shared/header/header'
import { Snackbar } from '../shared/snackbar/snackbar'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Header, Snackbar],
  templateUrl: './main.html',
})
export class Main {}
