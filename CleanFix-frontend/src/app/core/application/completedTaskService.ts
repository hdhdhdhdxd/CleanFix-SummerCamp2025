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

export const completedTaskService = {
  init,
  create,
}
