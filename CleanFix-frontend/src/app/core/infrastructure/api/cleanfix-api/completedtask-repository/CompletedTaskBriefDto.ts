export interface CompletedTaskBriefDto {
  id: number
  address: string
  companyName: string
  issueType: string
  isSolicitation: boolean
  creationDate: Date
  completionDate: Date
}
