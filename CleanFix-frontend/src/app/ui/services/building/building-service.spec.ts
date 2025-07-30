import { TestBed } from '@angular/core/testing'

import { provideZonelessChangeDetection } from '@angular/core'
import { BuildingService } from './building-service'

describe('BuildingService', () => {
  let service: BuildingService

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideZonelessChangeDetection()],
    })
    service = TestBed.inject(BuildingService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})
