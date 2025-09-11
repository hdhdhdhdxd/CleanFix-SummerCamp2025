import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core'
import { provideRouter, withComponentInputBinding, withViewTransitions } from '@angular/router'

import { provideHttpClient, withInterceptors } from '@angular/common/http'
import { routes } from './app.routes'
import { authInterceptor } from '@/ui/interceptors/auth/auth-interceptor'
import { AuthService } from '@/ui/services/auth/auth-service'
import { firstValueFrom } from 'rxjs'
import { RepositoryInitializerService } from '@/ui/services/repository-initializer.service'

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes, withComponentInputBinding(), withViewTransitions()),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAppInitializer(() => firstValueFrom(inject(AuthService).loadUser())),
    provideAppInitializer(() => {
      inject(RepositoryInitializerService)
      return Promise.resolve()
    }),
  ],
}
