import { Injectable, Type } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from '../base-http' ;
import { C } from "src/interfaces/constants";
import { Document, parseDocument, parseDocuments, QuotationParameters, SaleParameters } from "src/interfaces/document/Document";
import { Transaction } from "src/interfaces/document/Transaction";

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private service : service;

  private documentExtension : string = "document";
  private transactionExtension : string = "transaction";
  private pdfExtension : string = "pdf";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.docsControl, http);
  }

  public getDocuments(fn: (res: Document[]) => void) {
    this.service.getRequest(
      this.documentExtension,
      res => fn(parseDocuments(res.data as Document[]))
    )
  }

  public getDocument(documentId: number, fn: (res: Document) => void) {
    this.service.getRequest(
      this.concatUrl(this.documentExtension, documentId.toString()),
      res => fn(parseDocument(res.data as Document))
    );
  }

  public getTransaction(documentId: number, fn: (res: Transaction) => void ) {
    this.service.getRequest(
      this.concatUrl(this.transactionExtension, documentId.toString()),
      res => {
        let txn = res.data as Transaction;

        if (txn.document) {
          txn.document = parseDocument(txn.document);
        }

        fn(txn);
      }
    );
  }

  public setTransaction(documentId: number, key: string, fn: (res: any) => void) {
    this.service.postRequest(
      this.concatUrl(this.transactionExtension, "document"),
      { documentId, key },
      res => fn(res)
    );
  }

  public setApprove(
    documentId: number,
    approverDocId: number,
    approverId: number,
    status: string,
    fn: (res: any) => void,
    comments: string | undefined = undefined
  ) {
    this.service.postRequest(
      this.concatUrl(this.transactionExtension, "approve"),
      { documentId, approverDocId, approverId, status, comments },
      res => fn(res)
    );
  }

  public setSaleDocument(parameters: SaleParameters, fn: (res: any) => void) {
    this.service.postRequest(
      this.concatUrl(this.pdfExtension, 'sale'),
      parameters,
      fn
    );
  }

  public setQuotationDocument(parameters: QuotationParameters, fn: (res: any) => void) {
    this.service.postRequest(
      this.concatUrl(this.pdfExtension, 'quotation'),
      parameters,
      fn
    );
  }

  private concatUrl(base: string, ext: string) : string{
    return base + "/" + ext;
  }
}
