import { C } from "../constants";

export interface DocType {
  typeCode: string;
  id: number;
}

export interface SaleParameters {
  name?: string;
  place: string;
  date: Date;
  item: string;
  salesPlace: string;
  salesNum: number;
  salesAddress: string;
  salesSign: string;
  customerPlace: string;
  customerNum: number;
  customerAddress: string;
  customerSign: string;
  customerName: string;
  law: string;
  total: number;
  id?: number;
  docName: string
}

export interface ItemQuotation {
  id?: number;
  name?: string;
  code?: string;
  description?: string;
  qty?: number;
  value?: number;
}

export interface QuotationParameters {
  id?: number;
  docName?: string;
  date: string;
  duoDate: string;
  name: string;
  client: string;
  contact: string;
  notes: string;
  items: ItemQuotation[];
}


export interface File {
  parameters: any;
  url?: string,
  id: number;
}

export interface Document {
  name: string;
  docType: DocType;
  file: File;
  id: number;
}

export function parseDocuments(docs: Document[]) {
  return docs.map(x => parseDocument(x));
}

export function parseDocument(doc: Document) {
  doc.file = parseFile(doc.file, doc.id);
  return doc;
}

export function parseFile(file: File, docId: number) {
  file.url = C.urls.docsControl + "pdf/view/" + docId;
  return file;
}
