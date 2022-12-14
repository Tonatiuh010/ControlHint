import { Injectable, Type } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from './../base-http' ;
import { C } from "src/interfaces/constants";
import { Device, parseDevice, parseDevices } from "src/interfaces/catalog/Device";

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private service : service;
  private urlExtension : string = "device";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.flowControl, http);
  }

  public getDevices(fn: (res: Device[]) => void) {
    this.service.getRequest(
      this.urlExtension,
      res => fn( parseDevices(res.data as Device[]) )
    );
  }

  public getDevice(id: number, fn: (res: Device) => void) {
    this.service.getRequest(
      this.concatUrl(id.toString()),
      res => fn( parseDevice(res.data as Device) )
    );
  }

  private concatUrl(ext: string) : string{
    return this.urlExtension + "/" + ext;
  }
}
