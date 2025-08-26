export interface Solicitation {
  id: number
  address: string
  date: Date
  status: string
  issueType: string
  description?: string
  type?: string
  maintenanceCost?: number
}
