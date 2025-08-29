import { ComponentFixture, TestBed } from '@angular/core/testing'

import { IncidenceDialog } from './incidence-dialog'
import { provideZonelessChangeDetection } from '@angular/core'

describe('IncidenceDialog', () => {
  let component: IncidenceDialog
  let fixture: ComponentFixture<IncidenceDialog>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IncidenceDialog],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(IncidenceDialog)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
