import { Job } from "./Job";
import { Level } from "./Level";
import { Shift } from "./Shift";


export interface Employee {
  name: string;
  lastName: string;
  image: string;
  job: Job;
  accessLevels: Level[];
  shift: Shift;
  status: string;
  id: number;
}
