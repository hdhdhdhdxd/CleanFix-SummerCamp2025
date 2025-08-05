import { ComponentFixture, TestBed } from '@angular/core/testing'

import { ImageCard } from './image-card'
import { provideZonelessChangeDetection } from '@angular/core'

describe('ImageCard', () => {
  let component: ImageCard
  let fixture: ComponentFixture<ImageCard>

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImageCard],
      providers: [provideZonelessChangeDetection()],
    }).compileComponents()

    fixture = TestBed.createComponent(ImageCard)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})
