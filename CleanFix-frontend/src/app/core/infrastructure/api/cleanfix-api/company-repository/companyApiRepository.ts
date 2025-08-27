import { Company } from '@/core/domain/models/Company'
import { CompanyDto } from './CompanyDto'
import { environment } from 'src/environments/environment'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { PaginatedDataDto } from '../interfaces/PaginatedDataDto'
import { HttpParams } from '@angular/common/http'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
  typeIssueId?: number,
): Promise<PaginatedData<Company>> => {
  const queryParams = new HttpParams()
    .set('pageNumber', pageNumber.toString())
    .set('pageSize', pageSize.toString())
    .set('typeIssueId', typeIssueId != null ? typeIssueId.toString() : '0')

  const response = await fetch(environment.baseUrl + `companies/paginated?${queryParams}`)

  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }

  const responseJson: PaginatedDataDto<CompanyDto> = await response.json()

  return {
    items: responseJson.items.map((companyDto: CompanyDto) => ({
      id: companyDto.id,
      name: companyDto.name,
      number: companyDto.number,
      email: companyDto.email,
      issueType: companyDto.issueType,
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
