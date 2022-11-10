import { Department } from "./Department";


export interface Asset {
  name: string;
  inTime: string;
  outTime: string;
  lunchTime: string;
  dayCount: number;
  id?: number;
  department: Department;
//BIEN PENDIENTE NO LO OLVIDES
}
