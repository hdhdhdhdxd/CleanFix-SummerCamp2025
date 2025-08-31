import { IssueTypeDto } from '../common/interfaces/IssueTypeDto'

export interface IncidenceDto {
  id: number
  issueType: IssueTypeDto
  date: Date
  description: string
  address: string
  surface: number
  priority: number
}
