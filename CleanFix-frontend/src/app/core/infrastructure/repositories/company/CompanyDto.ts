export interface CompanyDto {
  id: number
  name: string
  address: string
  number: string
  email: string
  type: number
  price: number
  workTime: number
}

export enum IssueType {
  Fontaneria,
  Electricidad,
  Carpinteria,
  Pintura,
  Suelos,
  Limpieza,
  Listo,
}
