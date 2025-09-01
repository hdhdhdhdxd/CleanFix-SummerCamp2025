import { CompletedTaskBrief } from '@/core/domain/models/CompletedTaskBrief'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { environment } from 'src/environments/environment'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { CompletedTaskBriefDto } from './CompletedTaskBriefDto'
import { CompletedTaskDto } from './completedTaksDto'
import { CompletedTask } from '@/core/domain/models/CompletedTask'

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

  const response = await fetch(environment.baseUrl + '/completedtasks', {
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
  filterString?: string,
): Promise<PaginatedData<CompletedTaskBrief>> => {
  const params = new URLSearchParams({
    pageNumber: pageNumber.toString(),
    pageSize: pageSize.toString(),
  })

  if (filterString) {
    params.append('filterString', filterString)
  }

  const response = await fetch(
    `${environment.baseUrl}/completedtasks/paginated?${params.toString()}`,
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

const getById = async (id: string): Promise<CompletedTask> => {
  const response = await fetch(`${environment.baseUrl}/completedtasks/${id}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  })

  if (!response.ok) {
    throw new Error('Error al obtener la tarea completada')
  }

  const data: CompletedTaskDto = await response.json()

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

export const completedTaskApiRepository = {
  create,
  getPaginated,
  getById,
}
