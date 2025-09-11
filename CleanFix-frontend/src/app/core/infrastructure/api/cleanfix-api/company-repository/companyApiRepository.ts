import { Company } from '@/core/domain/models/Company'
import { CompanyDto } from './CompanyDto'
import { environment } from 'src/environments/environment'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { HttpClient, HttpParams } from '@angular/common/http'
import { firstValueFrom } from 'rxjs'
import { CompanyRepository } from '@/core/domain/repositories/CompanyRepository'

export class CompanyApiRepository implements CompanyRepository {
  constructor(private http: HttpClient) {}

  async getPaginated(
    pageNumber: number,
    pageSize: number,
    typeIssueId?: number,
  ): Promise<PaginatedData<Company>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())

    if (typeIssueId != null) {
      params.set('typeIssueId', typeIssueId.toString())
    }

    const responseJson = await firstValueFrom(
      this.http.get<PaginatedDataDto<CompanyDto>>(environment.baseUrl + 'companies/paginated', {
        params,
        withCredentials: true,
      }),
    )
    return {
      items: responseJson.items.map((companyDto: CompanyDto) => ({
        id: companyDto.id,
        name: companyDto.name,
        number: companyDto.number,
        price: companyDto.price,
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
}
