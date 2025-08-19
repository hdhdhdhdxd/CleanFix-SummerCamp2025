import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'
import { environment } from 'src/environments/environment'
import { SolicitationDto } from './SolicitationDto'
import { PaginationDto } from '../../../domain/models/PaginationDto'

const getAll = async (): Promise<PaginationDto<Solicitation>> => {
  const response = await fetch(environment.baseUrl + 'requests/paginated')
  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }

  const responseJson: PaginationDto<SolicitationDto> = await response.json()

  return {
    ...responseJson,
    items: responseJson.items.map((solicitation: SolicitationDto) => ({
      id: solicitation.id,
      address: solicitation.address,
      description: solicitation.description,
      type: solicitation.type,
      maintenanceCost: solicitation.maintenanceCost,
      date: new Date(solicitation.date),
      status: solicitation.status,
    })),
  }
}

export const solicitationApiRepository: SolicitationRepository = {
  getAll,
}
