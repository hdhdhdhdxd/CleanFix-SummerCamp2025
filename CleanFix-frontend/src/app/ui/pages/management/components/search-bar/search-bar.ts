import { Component, output, signal, input, OnInit } from '@angular/core'

@Component({
  selector: 'app-search-bar',
  imports: [],
  templateUrl: './search-bar.html',
})
export class SearchBar implements OnInit {
  searchChange = output<string>()
  initialValue = input<string>('')
  value = signal('')

  ngOnInit(): void {
    const initial = this.initialValue()
    if (initial) {
      this.value.set(initial)
    }
  }

  onInput(event: Event): void {
    const input = event.target as HTMLInputElement
    this.value.set(input.value.trim())
  }

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement
    this.value.set(input.value.trim())
  }

  onSubmit(event: Event): void {
    event.preventDefault()
    this.searchChange.emit(this.value())
  }

  clear(): void {
    this.value.set('')
    this.searchChange.emit('')
  }
}
