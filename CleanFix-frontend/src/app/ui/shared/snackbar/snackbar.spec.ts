import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Snackbar } from './snackbar'
import { provideZonelessChangeDetection } from '@angular/core'

describe('Snackbar', () => {
  let component: Snackbar
  let fixture: ComponentFixture<Snackbar>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Snackbar],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Snackbar)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
