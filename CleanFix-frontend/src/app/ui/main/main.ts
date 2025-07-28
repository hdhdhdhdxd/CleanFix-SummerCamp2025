import { Component } from '@angular/core'
import { RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'
import { Navbar } from "../shared/navbar/navbar";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Navbar],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {}
