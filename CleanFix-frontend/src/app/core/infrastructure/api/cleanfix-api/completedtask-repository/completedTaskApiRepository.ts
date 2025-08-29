import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { environment } from 'src/environments/environment'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { CompletedTaskBriefDto } from './CompletedTaskBriefDto'

const create = async (
  solicitationId: number,
  incidenceId: number,
  companyId: number,
  isSolicitation: boolean,
  materialIds: number[],
): Promise<void> => {
  const body = {
    solicitationId,
    incidenceId,
    companyId,
    isSolicitation,
    materialIds,
  }

  const response = await fetch(environment.baseUrl + 'completedtasks', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    throw new Error('Error al crear la tarea completada')
  }
}

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<CompletedTaskBrief>> => {
  const response = await fetch(
    `${environment.baseUrl}completedtasks/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    },
  )

  if (!response.ok) {
    throw new Error('Error al obtener las tareas completadas')
  }

  const data: PaginatedDataDto<CompletedTaskBriefDto> = await response.json()
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

export const completedTaskApiRepository = {
  create,
  getPaginated,
}
