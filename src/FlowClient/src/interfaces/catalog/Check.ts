import { Device } from "./Device";
import { Employee } from "./Employee";

export interface Check {
  device?: Device;
  employee?: Employee;
  checkDt: Date;
  checkType: string;
  id: number;
}

export interface Checks {
  dt: Date;
  day: string;
  in?: number;
  out?: string;
  couter: number;

}