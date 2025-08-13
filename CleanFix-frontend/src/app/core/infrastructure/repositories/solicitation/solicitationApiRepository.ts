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
    {
      id: 2,
      address: '456 Elm St',
      description: 'Install new light fixture',
      type: 'electricity',
      cost: 150,
      date: new Date(),
      status: 'completed',
    },
    {
      id: 3,
      address: '789 Oak St',
      description: 'Paint the living room',
      type: 'painting',
      cost: 200,
      date: new Date(),
      status: 'pending',
    },
    {
      id: 4,
      address: '101 Pine St',
      description: 'Clean the gutters',
      type: 'cleaning',
      cost: 75,
      date: new Date(),
      status: 'in_progress',
    },
  ]
}

export const solicitationApiRepository: SolicitationRepository = {
  getAll,
}
