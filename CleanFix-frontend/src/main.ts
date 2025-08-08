import { bootstrapApplication } from '@angular/platform-browser'
import { appConfig } from './app/config/app.config'
import { Main } from './app/ui/main/main'
import { initializeRepositories } from './app/config/repository-init'

initializeRepositories()

bootstrapApplication(Main, appConfig).catch((err) => console.error(err))
