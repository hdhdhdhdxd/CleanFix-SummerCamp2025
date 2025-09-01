export interface CompletedTask {
  id: number
  address: string
  creationDate: Date
  completionDate: Date
  price: number
  issueType: string
  isSolicitation: boolean
  surface: number
  materials: string[]
  company: {
    name: string
    address: string
    number: string
    email: string
  }
}
