import { Solicitation } from '../models/Solicitation'

export interface SolicitationRepository {
  getAll(): Promise<Solicitation[]>
}
