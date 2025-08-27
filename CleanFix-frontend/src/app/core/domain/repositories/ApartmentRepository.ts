export interface ApartmentRepository {
  getCount(buildingCode: string): Promise<number>
}
