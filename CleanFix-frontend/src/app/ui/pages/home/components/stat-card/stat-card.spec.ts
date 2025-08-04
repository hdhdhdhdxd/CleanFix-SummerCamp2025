import { ComponentFixture, TestBed } from '@angular/core/testing'

import { StatCard } from './stat-card'
import { provideZonelessChangeDetection } from '@angular/core'

describe('StatCard', () => {
  let component: StatCard
  let fixture: ComponentFixture<StatCard>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatCard],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(StatCard)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
