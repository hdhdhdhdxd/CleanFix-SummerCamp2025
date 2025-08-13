import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

const getAll = async (): Promise<Solicitation[]> => {
  return [
    {
      id: 1,
      address: '123 Main St',
      description: 'Fix the leaky faucet',
      type: 'plumbing',
      cost: 100,
      date: new Date(),
      status: 'canceled',
    },
  ]
}

export const solicitationApiRepository: SolicitationRepository = {
  getAll,
}
