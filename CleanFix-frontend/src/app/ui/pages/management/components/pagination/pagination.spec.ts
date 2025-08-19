import { ComponentFixture, TestBed } from '@angular/core/testing'

import { provideZonelessChangeDetection } from '@angular/core'
import { Pagination } from './pagination'

describe('Pagination', () => {
  let component: Pagination
  let fixture: ComponentFixture<Pagination>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Pagination],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Pagination)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
