export interface Company {
  id: number
  name?: string
  address?: string
  number?: string
  email?: string
  IssueType?: IssueType
  price?: number
  workTime?: number
}

export enum IssueType {
  Fontaneria = 'Fontanería',
  Electricidad = 'Electricidad',
  Carpinteria = 'Carpintería',
  Pintura = 'Pintura',
  Suelos = 'Suelos',
  Limpieza = 'Limpieza',
  Listo = 'Listo',
}
