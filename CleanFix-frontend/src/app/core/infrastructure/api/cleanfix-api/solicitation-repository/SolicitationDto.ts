import { IssueTypeDto } from '../interfaces/IssueTypeDto'

export interface SolicitationDto {
  id: string
  description: string
  date: Date
  address: string
  status: string
  maintenanceCost: number
  issueType: IssueTypeDto
}
