import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'
import { environment } from 'src/environments/environment'
import { PaginatedData } from '../../../domain/models/PaginatedData'
import { PaginatedDataDto } from '../../interfaces/PaginatedDataDto'
import { SolicitationDto } from './SolicitationDto'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<Solicitation>> => {
  const response = await fetch(
    environment.baseUrl + `solicitations/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}`,
  )
  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }

  const responseJson: PaginatedDataDto<SolicitationDto> = await response.json()

  return {
    items: responseJson.items.map((solicitation: SolicitationDto) => ({
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

export const solicitationApiRepository: SolicitationRepository = {
  getPaginated: getPaginated,
}
