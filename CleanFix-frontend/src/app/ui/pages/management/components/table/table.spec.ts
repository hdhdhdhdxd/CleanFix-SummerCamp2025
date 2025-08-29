import { ComponentFixture, TestBed } from '@angular/core/testing'

import { Table } from './table'
import { provideZonelessChangeDetection } from '@angular/core'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'

describe('Table', () => {
  let component: Table<SolicitationBrief>
  let fixture: ComponentFixture<Table<SolicitationBrief>>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Table],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(Table<SolicitationBrief>)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
