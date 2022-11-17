import { Injectable, Type } from "@angular/core";
import { combineLatest, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from "../base-http";
import { C } from "src/interfaces/constants";
import { Flow } from "src/interfaces/api/Api";

@Injectable({
  providedIn: 'root'
})

export class FlowService {
  private service : service;
  private urlExtension : string = "engine";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.flowControl, http);
  }

  public getFlows(fn: (res: Flow[]) => void) {
    this.service.getRequest(
      this.concatUrl("flow"),
      res => fn(res.data as Flow[])
    );
  }

  public getFlow(id: number, fn: (res: Flow) => void) {
    this.service.getRequest(
      this.concatUrl("flow/" + id.toString()),
      res => fn(res.data as Flow)
    );
  }

  public setDevFlow(deviceId: number, flowId: number, fn: (res: any) => void) {
    this.service.postRequest(
      this.concatUrl("devFlow"),
      { deviceId, flowId },
      res => fn(res)
    );
  }

  private concatUrl(ext: string) : string{
    return this.urlExtension + "/" + ext;
  }
}
