export interface CompletedTaskRepository {
  create(
    solicitationId: number,
    incidenceId: number,
    companyId: number,
    isSolicitation: boolean,
    MaterialIds: number[],
  ): Promise<void>
}
