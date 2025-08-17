import { ComponentFixture, TestBed } from '@angular/core/testing'

import { SolicitationDialog } from './solicitation-dialog'
import { provideZonelessChangeDetection } from '@angular/core'

describe('SolicitationDialog', () => {
  let component: SolicitationDialog
  let fixture: ComponentFixture<SolicitationDialog>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SolicitationDialog],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(SolicitationDialog)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
