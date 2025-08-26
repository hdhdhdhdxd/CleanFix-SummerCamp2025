import { Company } from '@/core/domain/models/Company'
import { CompanyDto, IssueType } from './CompanyDto'
import { environment } from 'src/environments/environment'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { PaginatedDataDto } from '../../interfaces/PaginatedDataDto'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<Company>> => {
  const response = await fetch(
    environment.baseUrl + `companies/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
  )
  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }

  const responseJson: PaginatedDataDto<CompanyDto> = await response.json()

  return {
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
    pageNumber: responseJson.pageNumber,
    totalPages: responseJson.totalPages,
    totalCount: responseJson.totalCount,
    hasPreviousPage: responseJson.hasPreviousPage,
    hasNextPage: responseJson.hasNextPage,
  }
}

export const companyApiRepository = {
  getPaginated,
}
