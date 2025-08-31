import { environment } from 'src/environments/environment'
import { IncidenceRepository } from '@/core/domain/repositories/IncidenceRepository'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'
import { IncidenceBriefDto } from './incidenceBriefDto'
import { priorities } from './priorityMapper'
import { IncidenceDto } from './IncidenceDto'
import { Incidence } from '@/core/domain/models/Incidence'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<IncidenceBrief>> => {
  const response = await fetch(
    environment.baseUrl + `/incidences/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
  )
  if (!response.ok) {
    throw new Error('Error al obtener las incidencias')
  }

  const responseJson: PaginatedDataDto<IncidenceBriefDto> = await response.json()

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

const getById = async (id: number): Promise<Incidence> => {
  const response = await fetch(environment.baseUrl + `/incidences/${id}`)
  if (!response.ok) {
    throw new Error('Error al obtener la incidencia')
  }

  const incidenceDto: IncidenceDto = await response.json()
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

export const incidenceApiRepository: IncidenceRepository = {
  getPaginated: getPaginated,
  getById: getById,
}
