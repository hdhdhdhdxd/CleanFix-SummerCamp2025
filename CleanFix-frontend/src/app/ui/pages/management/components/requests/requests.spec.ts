import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Requests } from './requests'
import { provideZonelessChangeDetection } from '@angular/core'

describe('Requests', () => {
  let component: Requests
  let fixture: ComponentFixture<Requests>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Requests],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Requests)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
