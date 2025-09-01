import { IssueType } from './IssueType'

export interface Incidence {
  id: number
  issueType: IssueType
  date: Date
  description: string
  address: string
  surface: number
  priority: string
}
