import { bootstrapApplication } from '@angular/platform-browser'
import { appConfig } from './app/config/app.config'
import { Main } from './app/ui/main/main'

bootstrapApplication(Main, appConfig).catch((err) => console.error(err))
