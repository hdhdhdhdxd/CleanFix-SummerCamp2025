import { Company } from '@/core/domain/models/Company'

const getAll = async (): Promise<Company[]> => {
  const response = await fetch('https://localhost:7096/api/companies')
  if (!response.ok) {
    throw new Error('Error al obtener las empresas')
  }
  return await response.json()
}

export const companyApiRepository = {
  getAll,
}
