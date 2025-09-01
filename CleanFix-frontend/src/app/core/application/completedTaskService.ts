import { CompletedTaskRepository } from '../domain/repositories/CompletedTaskRepository'

let completedTaskRepository: CompletedTaskRepository

const init = (repository: CompletedTaskRepository) => {
  completedTaskRepository = repository
}

const create = async (
  solicitationId: number,
  incidenceId: number,
  companyId: number,
  isSolicitation: boolean,
  MaterialIds: number[],
) => {
  return completedTaskRepository.create(
    solicitationId,
    incidenceId,
    companyId,
    isSolicitation,
    MaterialIds,
  )
}

const getPaginated = async (pageNumber: number, pageSize: number, filterString?: string) => {
  return completedTaskRepository.getPaginated(pageNumber, pageSize, filterString)
}

const getById = async (id: string) => {
  return completedTaskRepository.getById(id)
}

export const completedTaskService = {
  init,
  create,
  getPaginated,
  getById,
}
