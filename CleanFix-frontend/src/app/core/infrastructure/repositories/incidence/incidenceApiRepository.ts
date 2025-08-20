import { Incidence } from '@/core/domain/models/Incedence'
import { PaginationDto } from '../../../domain/models/PaginationDto'
import { environment } from 'src/environments/environment'
import { IncidenceDto } from './IncidenceDto'
import { IncidenceRepository } from '@/core/domain/repositories/IncidenceRepository'

const getAll = async (pageNumber: number, pageSize: number): Promise<PaginationDto<Incidence>> => {
  const response = await fetch(
    environment.baseUrl + `incidences/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
  )
  if (!response.ok) {
    throw new Error('Error al obtener las incidencias')
  }

  const responseJson: PaginationDto<IncidenceDto> = await response.json()

  return {
    ...responseJson,
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
  }
}

export const incidenceApiRepository: IncidenceRepository = {
  getAll,
}
