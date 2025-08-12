import { Solicitation } from '@/core/domain/models/Solicitation'
import { SolicitationRepository } from '@/core/domain/repositories/SolicitationRepository'

const getAll = async (): Promise<Solicitation[]> => {
  return [
    {
      id: '1',
      address: '123 Main St',
      description: 'Fix the leaky faucet',
      type: 'plumbing',
      cost: 100,
      date: new Date(),
      status: 'pending',
    },
    {
      id: '2',
      address: '456 Elm St',
      description: 'Paint the living room',
      type: 'painting',
      cost: 200,
      date: new Date(),
      status: 'completed',
    },
    {
      id: '3',
      address: '789 Oak St',
      description: 'Install new kitchen cabinets',
      type: 'carpentry',
      cost: 1500,
      date: new Date(),
      status: 'in_progress',
    },
  ]
}

export const solicitationApiRepository: SolicitationRepository = {
  getAll,
}
