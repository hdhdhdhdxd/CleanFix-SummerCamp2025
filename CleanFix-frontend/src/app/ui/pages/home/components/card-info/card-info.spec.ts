import { ComponentFixture, TestBed } from '@angular/core/testing'

import { CardInfo } from './card-info'

describe('CardInfo', () => {
  let component: CardInfo
  let fixture: ComponentFixture<CardInfo>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardInfo],
    }).compileComponents()

    fixture = TestBed.createComponent(CardInfo)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
