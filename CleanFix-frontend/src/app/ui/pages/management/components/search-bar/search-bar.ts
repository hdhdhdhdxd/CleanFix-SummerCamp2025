import { Component, output, signal } from '@angular/core'

@Component({
  selector: 'app-search-bar',
  imports: [],
  templateUrl: './search-bar.html',
})
export class SearchBar {
  searchChange = output<string>()
  value = signal('')

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
