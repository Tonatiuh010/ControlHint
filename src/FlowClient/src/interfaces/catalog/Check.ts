import { Device } from "./Device";
import { Employee } from "./Employee";

export interface Check {
  device?: Device;
  employee?: Employee;
  checkDt: Date;
  checkType: string;
  id: number;
}
