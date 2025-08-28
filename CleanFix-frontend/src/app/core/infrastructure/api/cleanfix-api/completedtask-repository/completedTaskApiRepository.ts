import { environment } from 'src/environments/environment'

const create = async (
  solicitationId: number,
  incidenceId: number,
  companyId: number,
  isSolicitation: boolean,
  materialIds: number[],
): Promise<void> => {
  const body = {
    solicitationId,
    incidenceId,
    companyId,
    isSolicitation,
    materialIds,
  }

  const response = await fetch(environment.baseUrl + 'completedtasks', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  })

  if (!response.ok) {
    throw new Error('Error al crear la tarea completada')
  }
}

export const completedTaskApiRepository = {
  create,
}
