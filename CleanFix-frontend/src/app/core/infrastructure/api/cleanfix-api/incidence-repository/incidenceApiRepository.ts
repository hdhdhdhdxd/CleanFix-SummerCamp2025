import { environment } from 'src/environments/environment'
import { IncidenceRepository } from '@/core/domain/repositories/IncidenceRepository'
import { PaginatedDataDto } from '../common/interfaces/PaginatedDataDto'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { IncidenceBrief } from '@/core/domain/models/IncidenceBrief'
import { IncidenceBriefDto } from './incidenceBriefDto'
import { priorities } from './priorityMapper'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<IncidenceBrief>> => {
  const response = await fetch(
    environment.baseUrl + `incidences/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
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

export const incidenceApiRepository: IncidenceRepository = {
  getPaginated: getPaginated,
}
