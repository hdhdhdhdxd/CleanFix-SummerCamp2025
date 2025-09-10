import { inject, Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { initializeRepositories } from '@/config/repository-init'

@Injectable({ providedIn: 'root' })
export class RepositoryInitializerService {
  private readonly http = inject(HttpClient)

  constructor() {
    initializeRepositories(this.http)
  }
}
