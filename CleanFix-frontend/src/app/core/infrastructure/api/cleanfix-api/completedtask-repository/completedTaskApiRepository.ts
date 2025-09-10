import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { environment } from 'src/environments/environment'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { CompletedTaskBriefDto } from './CompletedTaskBriefDto'
import { CompletedTaskDto } from './completedTaksDto'
import { CompletedTask } from '@/core/domain/models/CompletedTask'
import { HttpClient, HttpParams } from '@angular/common/http'
import { firstValueFrom } from 'rxjs'
import { CompletedTaskRepository } from '@/core/domain/repositories/CompletedTaskRepository'

export class CompletedTaskApiRepository implements CompletedTaskRepository {
  constructor(private http: HttpClient) {}

  async create(
    solicitationId: number,
    incidenceId: number,
    companyId: number,
    isSolicitation: boolean,
    materialIds: number[],
  ): Promise<void> {
    const body = {
      solicitationId,
      incidenceId,
      companyId,
      isSolicitation,
      materialIds,
    }
    await firstValueFrom(
      this.http.post(environment.baseUrl + 'completedtasks', body, {
        withCredentials: true,
        headers: { 'Content-Type': 'application/json' },
      }),
    )
  }

  async getPaginated(
    pageNumber: number,
    pageSize: number,
    filterString?: string,
  ): Promise<PaginatedData<CompletedTaskBrief>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
    if (filterString) {
      params = params.set('filterString', filterString)
    }
    const data = await firstValueFrom(
      this.http.get<PaginatedDataDto<CompletedTaskBriefDto>>(
        environment.baseUrl + 'completedtasks/paginated',
        { params, withCredentials: true },
      ),
    )
    return {
      items: data.items.map((item: CompletedTaskBriefDto) => ({
        id: item.id,
        address: item.address,
        companyName: item.companyName,
        issueType: item.issueType,
        isSolicitation: item.isSolicitation,
        creationDate: new Date(item.creationDate),
        completionDate: new Date(item.completionDate),
      })),
      pageNumber: data.pageNumber,
      totalPages: data.totalPages,
      totalCount: data.totalCount,
      hasPreviousPage: data.hasPreviousPage,
      hasNextPage: data.hasNextPage,
    }
  }

  async getById(id: string): Promise<CompletedTask> {
    const data = await firstValueFrom(
      this.http.get<CompletedTaskDto>(environment.baseUrl + 'completedtasks/' + id, {
        withCredentials: true,
      }),
    )
    return {
      id: data.id,
      address: data.address,
      issueType: data.issueType,
      isSolicitation: data.isSolicitation,
      creationDate: new Date(data.creationDate),
      completionDate: new Date(data.completionDate),
      company: {
        name: data.company.name,
        address: data.company.address,
        number: data.company.number,
        email: data.company.email,
      },
      price: data.price,
      surface: data.surface,
      materials: data.materials,
    }
  }
}
