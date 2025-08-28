import { TestBed } from '@angular/core/testing'

import { CompletedTaskService } from './completed-task-service'

describe('CompletedTaskService', () => {
  let service: CompletedTaskService

  beforeEach(() => {
    TestBed.configureTestingModule({})
    service = TestBed.inject(CompletedTaskService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})
