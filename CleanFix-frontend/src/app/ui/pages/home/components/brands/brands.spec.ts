import { ComponentFixture, TestBed } from '@angular/core/testing'

import { provideZonelessChangeDetection } from '@angular/core'
import { Brands } from './brands'

describe('Brands', () => {
  let component: Brands
  let fixture: ComponentFixture<Brands>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Brands],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Brands)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
