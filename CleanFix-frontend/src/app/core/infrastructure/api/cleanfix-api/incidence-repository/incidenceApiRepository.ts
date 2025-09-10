import { environment } from 'src/environments/environment'
import { IncidenceRepository } from '@/core/domain/repositories/IncidenceRepository'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'
import { IncidenceBriefDto } from './incidenceBriefDto'
import { priorities } from './priorityMapper'
import { IncidenceDto } from './IncidenceDto'
import { Incidence } from '@/core/domain/models/Incidence'
import { HttpClient, HttpParams } from '@angular/common/http'
import { firstValueFrom } from 'rxjs'

export class IncidenceApiRepository implements IncidenceRepository {
  constructor(private http: HttpClient) {}

  async getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Promise<PaginatedData<IncidenceBrief>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
    if (filterString) {
      params = params.set('filterString', filterString)
    }
    const responseJson = await firstValueFrom(
      this.http.get<PaginatedDataDto<IncidenceBriefDto>>(
        `${environment.baseUrl}incidences/paginated`,
        { params, withCredentials: true },
      ),
    )
    return {
      items: responseJson.items.map((incidenceDto: IncidenceBriefDto) => ({
        id: incidenceDto.id,
        address: incidenceDto.address,
        date: new Date(incidenceDto.date),
        issueType: incidenceDto.issueType,
        priority: priorities[incidenceDto.priority],
      })),
      pageNumber: responseJson.pageNumber,
      totalPages: responseJson.totalPages,
      totalCount: responseJson.totalCount,
      hasPreviousPage: responseJson.hasPreviousPage,
      hasNextPage: responseJson.hasNextPage,
    }
  }

  async getById(id: number): Promise<Incidence> {
    const incidenceDto = await firstValueFrom(
      this.http.get<IncidenceDto>(environment.baseUrl + `incidences/${id}`, {
        withCredentials: true,
      }),
    )
    return {
      id: incidenceDto.id,
      issueType: incidenceDto.issueType,
      date: new Date(incidenceDto.date),
      description: incidenceDto.description,
      address: incidenceDto.address,
      surface: incidenceDto.surface,
      priority: priorities[incidenceDto.priority],
    }
  }
}
