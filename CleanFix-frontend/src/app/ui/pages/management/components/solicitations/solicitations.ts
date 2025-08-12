import { Component } from '@angular/core'
import { SearchBar } from '../search-bar/search-bar'
import { Table } from '../table/table'

@Component({
  selector: 'app-solicitations',
  imports: [SearchBar, Table],
  templateUrl: './solicitations.html',
})
export class Solicitations {}
