import { IssueType } from './IssueType'

export interface Solicitation {
  id: number
  address: string
  date: Date
  status: string
  issueType: IssueType
  description: string
  type: string
  maintenanceCost: number
}
