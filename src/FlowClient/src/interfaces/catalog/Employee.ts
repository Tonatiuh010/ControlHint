import { Asset } from "./Asset";
import { Job } from "./Job";
import { Shift } from "./Shift";


export interface Employee {
  name: string;
  lastName: string;
  /* image: Image; */
  job: Job;
  accessLevels: any[];
  shift: Shift;
  status: string;
  id: number;
}
