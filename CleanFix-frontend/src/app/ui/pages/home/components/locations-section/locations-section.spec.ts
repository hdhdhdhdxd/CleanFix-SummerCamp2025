import { ComponentFixture, TestBed } from '@angular/core/testing'

import { LocationsSection } from './locations-section'
import { provideZonelessChangeDetection } from '@angular/core'

describe('LocationsSection', () => {
  let component: LocationsSection
  let fixture: ComponentFixture<LocationsSection>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LocationsSection],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(LocationsSection)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
