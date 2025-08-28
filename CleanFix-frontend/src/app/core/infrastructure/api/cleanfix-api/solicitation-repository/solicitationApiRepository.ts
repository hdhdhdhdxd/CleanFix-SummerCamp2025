import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'
import { environment } from 'src/environments/environment'
import { SolicitationBriefDto } from './SolicitationBriefDto'
import { PaginatedData } from '@/core/domain/models/PaginatedData'
import { PaginatedDataDto } from '../interfaces/PaginatedDataDto'
import { SolicitationBrief } from '@/core/domain/models/SolicitationBrief'
import { SolicitationDto } from './SolicitationDto'

const getPaginated = async (
  pageNumber: number,
  pageSize: number,
): Promise<PaginatedData<SolicitationBrief>> => {
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

  const responseJson: SolicitationDto = await response.json()

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

export const solicitationApiRepository: SolicitationRepository = {
  getPaginated: getPaginated,
  getById: getById,
}
