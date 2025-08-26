import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'
import { environment } from 'src/environments/environment'
import { PaginatedData } from '../../../domain/models/PaginatedData'
import { PaginatedDataDto } from '../../interfaces/PaginatedDataDto'
import { SolicitationBriefDto } from './SolicitationBriefDto'

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

  const responseJson: PaginatedDataDto<SolicitationBriefDto> = await response.json()

  return {
    items: responseJson.items.map((solicitation: SolicitationBriefDto) => ({
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

const getById = async (id: number): Promise<Solicitation> => {
  const response = await fetch(environment.baseUrl + `solicitations/${id}`)
  if (!response.ok) {
    throw new Error('Error al obtener la solicitud')
  }

  const responseJson: SolicitationBriefDto = await response.json()

  return {
    id: responseJson.id,
    address: responseJson.address,
    date: new Date(responseJson.date),
    status: responseJson.status,
    issueType: responseJson.issueType,
  }
}

export const solicitationApiRepository: SolicitationRepository = {
  getPaginated: getPaginated,
  getById: getById,
}
