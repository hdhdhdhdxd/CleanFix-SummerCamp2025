import { Incidence } from '@/core/domain/models/Incedence'
import { PaginatedData } from '../../../domain/models/PaginatedData'
import { environment } from 'src/environments/environment'
import { IncidenceDto } from './IncidenceDto'
import { IncidenceRepository } from '@/core/domain/repositories/IncidenceRepository'
import { PaginatedDataDto } from '../../interfaces/PaginatedDataDto'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<Incidence>> => {
  const response = await fetch(
    environment.baseUrl + `incidences/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
  )
  if (!response.ok) {
    throw new Error('Error al obtener las incidencias')
  }

  const responseJson: PaginatedDataDto<IncidenceDto> = await response.json()

  return {
    items: responseJson.items.map((incidenceDto: IncidenceDto) => ({
      id: incidenceDto.id,
      type: incidenceDto.type,
      date: new Date(incidenceDto.date),
      status: incidenceDto.status,
      description: incidenceDto.description,
      apartmentId: incidenceDto.apartmentId,
      surface: incidenceDto.surface,
      priority: incidenceDto.priority,
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
