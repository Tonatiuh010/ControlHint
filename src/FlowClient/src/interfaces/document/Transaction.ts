import { ApproverStep } from "./Approver";
import { Document } from "./Document";

export interface Transaction {
  document?: Document,
  approvers?: ApproverStep[]
}
