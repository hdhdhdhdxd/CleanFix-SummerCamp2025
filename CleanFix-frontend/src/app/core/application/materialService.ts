import { MaterialRepository } from '../domain/repositories/MaterialRepository'

let materialRepository: MaterialRepository

const init = (repository: MaterialRepository) => {
  materialRepository = repository
}

const getRandomThree = async (issueType: number) => {
  return materialRepository.getRandomThree(issueType)
}

export const materialService = {
  init,
  getRandomThree,
}
