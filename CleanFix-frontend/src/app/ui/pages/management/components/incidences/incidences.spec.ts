import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Incidences } from './incidences'
import { provideZonelessChangeDetection } from '@angular/core'

describe('Incidences', () => {
  let component: Incidences
  let fixture: ComponentFixture<Incidences>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Incidences],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Incidences)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
