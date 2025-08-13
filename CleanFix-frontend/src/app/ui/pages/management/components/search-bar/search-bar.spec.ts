import { ComponentFixture, TestBed } from '@angular/core/testing'

import { SearchBar } from './search-bar'
import { provideZonelessChangeDetection } from '@angular/core'

describe('SearchBar', () => {
  let component: SearchBar
  let fixture: ComponentFixture<SearchBar>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchBar],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(SearchBar)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
