import { Department } from "./Department";

export interface Position {
  positionId: number;
  alias?: string;
  department?: Department;
  name?: string;
  description?: string;
  id?: number;
}
