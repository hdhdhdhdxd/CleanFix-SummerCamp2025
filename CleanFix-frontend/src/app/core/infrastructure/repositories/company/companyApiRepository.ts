import { Company } from '@/core/domain/models/Company'
import { CompanyDto, IssueType } from './CompanyDto'
import { environment } from 'src/environments/environment'
import { PaginationDto } from '@/core/domain/models/PaginationDto'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginationDto<Company>> => {
  const response = await fetch(
    environment.baseUrl + `companies/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
  )
  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }

  const responseJson = await response.json()

  return {
    ...responseJson,
    items: responseJson.items.map((companyDto: CompanyDto) => ({
      id: companyDto.id,
      name: companyDto.name,
      address: companyDto.address,
      number: companyDto.number,
      email: companyDto.email,
      type: IssueType[companyDto.type],
      price: companyDto.price,
      workTime: companyDto.workTime,
    })),
  }
}

export const companyApiRepository = {
  getPaginated,
}
