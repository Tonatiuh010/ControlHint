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

export class ApproverService {
  private service: service;
  private urlExtension : string = "Approver";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.docsControl, http);
  }

  //SECCION DE APPROVER
  public getApprovers(fn: (res: any) => void) {
    this.service.getRequest(
      this.urlExtension,
      res => {
        fn(res.data as Approver[])
      }
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
      this.concatUrl("docsApprover"),
      res => fn(res.data as ApproverDocument[])
    );
  }

  public getApproverDocument(id: number, fn: (res: ApproverDocument) => void) {
    this.service.getRequest(
      this.concatUrl("docsApprover/" + id.toString()),
      res => fn(res.data as ApproverDocument)
    );
  }

  public getApproverDocumentsByType(department: string, type: string, fn: (res: ApproverDocument[]) => void) {
    this.getApproverDocuments(
      res => {
        let docs = res.filter(x => x.docFlow.docType.typeCode == type && x.docFlow.key1 == department);
        fn(docs);
      }
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
      this.concatUrl("docsApprover"),
      { id },
      res => fn(res)
    );
  }

  //SET APPROVER DOCUMENT
  private concatUrl(ext: string) : string{
    return this.urlExtension + "/" + ext;
  }

}
