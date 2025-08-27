import { IssueTypeDto } from '../interfaces/IssueTypeDto'

export interface SolicitationDto {
  id: number
  description: string
  date: Date
  address: string
  status: string
  maintenanceCost: number
  issueType: IssueTypeDto
  buildingCode: string
  apartmentCount: number
}
