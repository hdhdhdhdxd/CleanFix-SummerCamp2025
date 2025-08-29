import { Component, inject } from '@angular/core'
import { NavigationEnd, Router, RouterOutlet } from '@angular/router'
import { Footer } from '../shared/footer/footer'
import { Header } from '../shared/header/header'
import { Snackbar } from '../shared/snackbar/snackbar'

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Footer, Header, Snackbar],
  templateUrl: './main.html',
})
export class Main {
  showFooter = true

  private router = inject(Router)

  constructor() {
    this.router.events.subscribe((event: unknown) => {
      if (event instanceof NavigationEnd) {
        // Oculta el footer en la ruta /service-request
        this.showFooter = !event.urlAfterRedirects.startsWith('/service-request')
      }
    })
  }
}
