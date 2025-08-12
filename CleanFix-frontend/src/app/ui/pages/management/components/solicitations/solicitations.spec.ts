import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Solicitations } from './solicitations'
import { provideZonelessChangeDetection } from '@angular/core'

describe('Solicitations', () => {
  let component: Solicitations
  let fixture: ComponentFixture<Solicitations>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Solicitations],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Solicitations)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
