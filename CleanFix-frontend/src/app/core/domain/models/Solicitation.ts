import { IssueType } from './IssueType'

export interface Solicitation {
  id: number
  address: string
  date: Date
  status: string
  issueType: IssueType
  description: string
  maintenanceCost: number
  buildingCode: string
  apartmentCount: number
}
