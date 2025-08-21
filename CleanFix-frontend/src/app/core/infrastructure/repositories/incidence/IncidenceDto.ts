export interface IncidenceDto {
  id: number
  type: IssueType
  date: Date
  status: string
  description: string
  apartmentId: number
  surface: number
  priority: Priority
}

export enum IssueType {
  PLUMBING = 'PLUMBING',
  ELECTRICAL = 'ELECTRICAL',
  HVAC = 'HVAC',
  GENERAL_MAINTENANCE = 'GENERAL_MAINTENANCE',
  OTHER = 'OTHER',
}

export enum Priority {
  LOW = 'LOW',
  MEDIUM = 'MEDIUM',
  HIGH = 'HIGH',
}
