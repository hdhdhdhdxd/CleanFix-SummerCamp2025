import { Company } from '@/core/domain/models/Company'
import { CompanyDto, IssueType } from './CompanyDto'

const getAll = async (): Promise<Company[]> => {
  const response = await fetch('https://localhost:7096/api/companies')
  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }

  const responseJson = await response.json()

  return responseJson.map((company: CompanyDto) => ({
    id: company.id,
    name: company.name,
    address: company.address,
    number: company.number,
    email: company.email,
    type: IssueType[company.type],
    price: company.price,
    workTime: company.workTime,
  }))
}

export const companyApiRepository = {
  getAll,
}
