import { ComponentFixture, TestBed } from '@angular/core/testing'

import { provideZonelessChangeDetection } from '@angular/core'
import { Hero } from './hero'

describe('Hero', () => {
  let component: Hero
  let fixture: ComponentFixture<Hero>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Hero],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Hero)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
