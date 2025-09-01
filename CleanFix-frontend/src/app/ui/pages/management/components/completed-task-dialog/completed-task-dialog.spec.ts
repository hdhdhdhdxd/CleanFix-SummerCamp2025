import { ComponentFixture, TestBed } from '@angular/core/testing'

import { CompletedTaskDialog } from './completed-task-dialog'
import { provideZonelessChangeDetection } from '@angular/core'

describe('CompletedTaskDialog', () => {
  let component: CompletedTaskDialog
  let fixture: ComponentFixture<CompletedTaskDialog>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompletedTaskDialog],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(CompletedTaskDialog)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
