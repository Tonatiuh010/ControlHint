import { Component, Input, OnInit } from '@angular/core';
import { Department } from "src/interfaces/catalog/Department";
import { DepartmentService } from "src/app/services/requests/departments.service";
import { FormControl } from '@angular/forms';
import { DocumentService } from "src/app/services/requests/document.service";
import { Document } from "src/interfaces/document/Document";
import { C } from 'src/interfaces/constants';
import { ApproverService } from 'src/app/services/requests/approver.service';
import { ApproverDocument } from 'src/interfaces/document/Approver';



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
  approverDocuments?: ApproverDocument[];


  constructor(
    private DepartmentService : DepartmentService,
    private ApproverService : ApproverService,
  ) { }

  ngOnInit(): void {
    this.selectorType.setValue(C.DocumentType.Quotation);

    this.DepartmentService.getDepartments((department)  => {
      this.departments = department;
      if(this.departments){
        this.selectorDepartment.setValue(this.departments[0].code);
        this.searchFlow();
      }
    });
  }

  searchFlow(){
    this.ApproverService.getApproverDocumentsByType(this.selectorDepartment.value, this.selectorType.value, res => {
      if(res){
        if(res.length > 0) {
          this.approverDocuments = res;
        }
      }
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
