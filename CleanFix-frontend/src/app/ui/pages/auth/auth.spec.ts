import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Auth } from './auth'
import { provideZonelessChangeDetection } from '@angular/core'

describe('Auth', () => {
  let component: Auth
  let fixture: ComponentFixture<Auth>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Auth],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Auth)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
