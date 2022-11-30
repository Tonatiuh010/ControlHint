import { Position } from "../catalog/Position";
import { DocType } from "./Document";

export interface DocFlow {
  docType: DocType;
  key1: string;
  key2: string;
  key3: string;
  key4: string;
  id: number;
}

export interface ApproverStep {
  documentDetail?: ApproverDocument;
  status: string;
  depto: string;
  comments: string;
}

export interface ApproverDocument {
  docFlow: DocFlow;
  approver: Approver;
  sequence: number;
  name: string;
  action: string;
  id: number;
}

export interface Approver {
  fullName: string;
  position: Position;
  id: number;
}
