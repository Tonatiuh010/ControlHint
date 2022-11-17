import { Flow } from "./../api/Api"

export interface Device {
  name: string;
  ip: string;
  model: string;
  isActive: boolean;
  last_update: Date;
  id: number;
  flow?: Flow
}

export function parseDevice(d: Device) : Device {
  if (typeof d.last_update == "string") {
    d.last_update = new Date(d.last_update);
  }

  return d;
}

export function parseDevices(dList: Device[]) : Device[] {
  return dList.map(d => parseDevice(d));
}
