import { HttpClient, HttpParams } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'
import { SolicitationBriefDto } from './SolicitationBriefDto'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationDto } from './SolicitationDto'

export class SolicitationApiRepository implements SolicitationRepository {
  constructor(private http: HttpClient) {}

  async getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Promise<PaginatedData<SolicitationBrief>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
    if (filterString) {
      params = params.set('filterString', filterString)
    }

    const responseJson = await this.http
      .get<
        PaginatedDataDto<SolicitationBriefDto>
      >(`${environment.baseUrl}solicitations/paginated`, { params })
      .toPromise()

    if (!responseJson) {
      throw new Error('No se pudo obtener la paginación de solicitudes')
    }

    return {
      items: (responseJson.items || []).map((solicitation: SolicitationBriefDto) => ({
        id: solicitation.id,
        address: solicitation.address,
        date: new Date(solicitation.date),
        status: solicitation.status,
        issueType: solicitation.issueType,
      })),
      pageNumber: responseJson.pageNumber,
      totalPages: responseJson.totalPages,
      totalCount: responseJson.totalCount,
      hasPreviousPage: responseJson.hasPreviousPage,
      hasNextPage: responseJson.hasNextPage,
    }
  }

  async getById(id: number): Promise<Solicitation> {
    const responseJson = await this.http
      .get<SolicitationDto>(`${environment.baseUrl}solicitations/${id}`)
      .toPromise()

    if (!responseJson) {
      throw new Error('No se pudo obtener la solicitud')
    }

    return {
      id: responseJson.id,
      address: responseJson.address,
      date: new Date(responseJson.date),
      status: responseJson.status,
      issueType: responseJson.issueType,
      description: responseJson.description,
      maintenanceCost: responseJson.maintenanceCost,
      buildingCode: responseJson.buildingCode,
      apartmentAmount: responseJson.apartmentAmount,
    }
  }
}
