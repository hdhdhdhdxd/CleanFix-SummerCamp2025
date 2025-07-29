import { ComponentFixture, TestBed } from '@angular/core/testing'

import { provideZonelessChangeDetection } from '@angular/core'
import { ServiceCard } from './service-card'

describe('ServiceCard', () => {
  let component: ServiceCard
  let fixture: ComponentFixture<ServiceCard>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceCard],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(ServiceCard)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
