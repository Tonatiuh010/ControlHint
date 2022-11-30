import { Component, Input, OnInit } from '@angular/core';
import { Department } from "src/interfaces/catalog/Department";
import { DepartmentService } from "src/app/services/requests/departments.service";
import { FormControl } from '@angular/forms';
import { DocumentService } from "src/app/services/requests/document.service";
import { Document } from "src/interfaces/document/Document";
import { C } from 'src/interfaces/constants';


@Component({
  selector: 'app-doc-flow',
  templateUrl: './doc-flow.component.html',
})
export class DocFlowComponent implements OnInit {
  selectorDepartment: FormControl = new FormControl();
  selectorType: FormControl = new FormControl();

  showModalApproved?: boolean = false;
  showModalFlow?: boolean = false;

  departments?: Department[];
  documents?: Document[];

  constructor(
    private DepartmentService : DepartmentService,
    private DocumentService : DocumentService,
  ) { }

  ngOnInit(): void {
    this.selectorType.setValue(C.DocumentType.Quotation);

    this.DepartmentService.getDepartments((department)  => {
      this.departments = department;
    });
    this.DocumentService.getDocuments((document) => {
      this.documents = document;
    })
  }

  selectType() {
    let value = this.selectorType.value;

    if(value) {
      this.selectType();
    }
  }

  showApproved(){
    this.showModalApproved = true;
  }

  showFlow(){
    this.showModalFlow = true;
  }

  closeFlow(){
    this.showModalFlow = false;
  }

  closeApproved(){
    this.showModalApproved = false;
  }

}
