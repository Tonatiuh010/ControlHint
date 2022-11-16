import { Position } from "./Position";
import { Level } from "./Level";
import { Shift } from "./Shift";


export interface Employee {
  name: string;
  lastName: string;
  image?: string;
  position: Position;
  accessLevels: Level[];
  shift: Shift;
  status: string;
  id: number;
}
