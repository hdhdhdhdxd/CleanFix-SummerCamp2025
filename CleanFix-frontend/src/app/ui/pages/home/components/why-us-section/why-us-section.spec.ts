import { ComponentFixture, TestBed } from '@angular/core/testing'

import { WhyUsSection } from './why-us-section'
import { provideZonelessChangeDetection } from '@angular/core'

describe('WhyUsSection', () => {
  let component: WhyUsSection
  let fixture: ComponentFixture<WhyUsSection>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WhyUsSection],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(WhyUsSection)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
