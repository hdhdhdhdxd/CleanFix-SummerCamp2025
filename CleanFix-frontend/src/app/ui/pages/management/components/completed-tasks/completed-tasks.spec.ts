import { ComponentFixture, TestBed } from '@angular/core/testing'

import { CompletedTasks } from './completed-tasks'
import { provideZonelessChangeDetection } from '@angular/core'

describe('CompletedTasks', () => {
  let component: CompletedTasks
  let fixture: ComponentFixture<CompletedTasks>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompletedTasks],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(CompletedTasks)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
