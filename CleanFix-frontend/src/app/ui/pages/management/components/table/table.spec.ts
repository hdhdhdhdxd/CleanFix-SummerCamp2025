import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Table } from './table'
import { provideZonelessChangeDetection } from '@angular/core'

describe('Table', () => {
  type TestTableRow = Record<string, string | number | Date>
  let component: Table<TestTableRow>
  let fixture: ComponentFixture<Table<TestTableRow>>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Table],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Table)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
