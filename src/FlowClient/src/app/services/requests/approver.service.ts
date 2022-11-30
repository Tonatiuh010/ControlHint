import { Injectable, Type } from "@angular/core";
import { BaseHttp as service } from "../base-http";
import { HttpClient } from "@angular/common/http";
import { C } from "src/interfaces/constants";
import { combineLatest, concatMap, Observable, startWith } from "rxjs";
import { Approver } from "src/interfaces/document/Approver";
import { ApproverDocument } from "src/interfaces/document/Approver";


@Injectable({
  providedIn: 'root'
})

export class ApproverServices {
  private service: service;
  private urlExtension : string = "approver";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.accessControl, http);
  }

  //SECCION DE APPROVER
  public getApprovers(fn: (res: Approver[]) => void) {
    this.service.getRequest(
      this.urlExtension,
      res => fn(res.data as Approver[])
    );
  }

  public getApprover(id: number, fn: (res: Approver) => void) {
    this.service.getRequest(
      this.concatUrl(id.toString()),
      res => fn(res.data as Approver)
    );
  }

  //SECCION DE APPROVER DOCUMENT
  public getApproverDocuments(fn: (res: ApproverDocument[]) => void) {
    this.service.getRequest(
      this.concatUrl("appproverDocument"),
      res => fn(res.data as ApproverDocument[])
    );
  }

  public getApproverDocument(id: number, fn: (res: ApproverDocument) => void) {
    this.service.getRequest(
      this.concatUrl("appproverDocument/" + id.toString()),
      res => fn(res.data as ApproverDocument)
    );
  }

  //SECCION SET
  //SET APPROVER
  public setApprover(id: number, fn: (res: any) => void) {
    this.service.postRequest(
      this.urlExtension,
      { id },
      res => fn(res)
    );
  }

  public setApproverDocument(id: number, fn: (res: any) => void) {
    this.service.postRequest(
      this.concatUrl("appproverDocument"),
      { id },
      res => fn(res)
    );
  }

  //SET APPROVER DOCUMENT
  private concatUrl(ext: string) : string{
    return this.urlExtension + "/" + ext;
  }

}
