import { ComponentFixture, TestBed } from '@angular/core/testing'

import { WhyUsCard } from './why-us-card'
import { provideZonelessChangeDetection } from '@angular/core'

describe('WhyUsCard', () => {
  let component: WhyUsCard
  let fixture: ComponentFixture<WhyUsCard>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WhyUsCard],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(WhyUsCard)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
