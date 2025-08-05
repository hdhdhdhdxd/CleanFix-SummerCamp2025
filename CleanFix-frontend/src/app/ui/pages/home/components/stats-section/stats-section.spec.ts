import { ComponentFixture, TestBed } from '@angular/core/testing'

import { StatsSection } from './stats-section'
import { provideZonelessChangeDetection } from '@angular/core'

describe('StatsSection', () => {
  let component: StatsSection
  let fixture: ComponentFixture<StatsSection>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatsSection],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(StatsSection)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
