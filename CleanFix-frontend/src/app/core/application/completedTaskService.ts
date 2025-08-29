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

const getPaginated = async (pageNumber: number, pageSize: number) => {
  return completedTaskRepository.getPaginated(pageNumber, pageSize)
}

export const completedTaskService = {
  init,
  create,
  getPaginated,
}
