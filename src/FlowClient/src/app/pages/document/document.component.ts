import { Component, OnInit, ElementRef, Output, EventEmitter, Type } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device, parseDevice } from 'src/interfaces/catalog/Device';
import { Transaction as Txn} from 'src/interfaces/document/Transaction'
import { Document as _Doc, QuotationParameters, SaleParameters } from 'src/interfaces/document/Document'
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { C } from 'src/interfaces/constants';
import { DocumentService } from 'src/app/services/requests/document.service';
import { FormControl } from '@angular/forms';
import { parseDocument } from 'src/interfaces/document/Document';
import { Department } from 'src/interfaces/catalog/Department';
import { DepartmentService } from 'src/app/services/requests/departments.service';

@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
})
export class DocumentComponent implements OnInit {
  noImg : string = C['no-image'];
  width: number = 0;
  docId? : number;
  pdfSrc? : string;

  docType: string = C.DocumentType.Sale;
  transactionDocument? : Txn;
  parameters?: SaleParameters | QuotationParameters;
  departaments? : Department[];

  selectorType: FormControl = new FormControl();
  selectorDepto: FormControl = new FormControl();

  isEditable: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private service: DocumentService,
    private deptoService : DepartmentService
  ) {
  }

  ngOnInit(): void {
    const observer = new ResizeObserver(entries => {
      entries.forEach(entry => {
        this.setPdfContainerSize(entry.contentRect.width, entry.contentRect.height);
      });
    });

    observer.observe(document.querySelector("#pdf-view") as Element);

    this.route.queryParams.subscribe(params => {
      this.docId = params['id'];

      if(this.docId) {
        this.getTransaction();
      } else {
        this.setType(C.DocumentType.Quotation)
      }
    });

    this.deptoService.getDepartments(deptos => {
      this.departaments = deptos;
      if(this.departaments && this.departaments.length > 0 ) {
        let depto = this.departaments[0];
        this.selectorDepto.setValue(depto.code);
      }
    });

    this.selectorType.setValue(C.DocumentType.Quotation);
  }

  getTransaction() {
    if(this.docId) {
      this.service.getTransaction(
        this.docId,
        txn => this.setTransaction(txn)
      )
    }
  }

  setTransaction(txn: Txn) {
    this.transactionDocument = txn;
    let document = this.transactionDocument.document;

    if (document) {
      this.docId = document.id;
      this.transactionDocument.document = parseDocument(this.transactionDocument.document as _Doc);
      this.preparePdf();
      this.setType(document.docType.typeCode);
      this.setParameters(document.file.parameters);
    }

  }

  selectType() {
    let value = this.selectorType.value;

    if(value) {
      this.setType(value);
    }
  }

  selectDevice(device: Device) {
  }

  preparePdf() {
    this.pdfSrc = C.urls.docsControl + 'pdf/view/' + this.docId;
  }

  setType(type: string) {
    this.docType = type;
  }

  setParameters(parameters: SaleParameters | QuotationParameters) {
    this.parameters = parameters;

    if(this.docId && this.transactionDocument) {
      let document = this.transactionDocument?.document;
      if(document) {
        let file = document.file;
        if(file) {
          file.parameters = parameters;
          parameters.id = this.docId;
        }
      }
    } else {
    }

    if("place" in this.parameters) {
      this.service.setSaleDocument(this.parameters, res => {
        console.log(res);
      });
    } else if("duoDate" in this.parameters) {
      this.service.setQuotationDocument(this.parameters, res => {
        console.log(res);
      });
    }

    if(this.transactionDocument) {
      let approvers = this.transactionDocument.approvers;
      if( approvers) {
        if(approvers.length == 0) {

        }
      }
    }

  }

  private sendTxn(txn: Txn) {
    let key = this.selectorDepto.value;

    if (key) {
      this.service.setTransaction(
        txn.document?.id as number,
        key,
        res => {
          console.log(res);
      });
    }
  }

  private setPdfContainerSize(width: number, height: number) {
    this.setWidth(width);
  }

  private setWidth(width: number) {
    this.width = width - 15;
  }
}
