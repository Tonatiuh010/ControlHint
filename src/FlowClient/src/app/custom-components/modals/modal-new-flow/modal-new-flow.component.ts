import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Department } from 'src/interfaces/catalog/Department';
import { DepartmentService } from "src/app/services/requests/departments.service";
import { Document } from "src/interfaces/document/Document";
import { C } from 'src/interfaces/constants';

@Component({
  selector: 'app-modal-new-flow',
  templateUrl: './modal-new-flow.component.html'
})
export class ModalNewFlowComponent implements OnInit, OnChanges {
  selectorType: FormControl = new FormControl();
  selectorDepartment: FormControl = new FormControl();

  departments?: Department[];

  @Input() isNew?: boolean;
  @Output() onCloseModal = new EventEmitter();

  constructor(
    private DepartmentService : DepartmentService,
  ) {
   }

  ngOnInit(): void {
    this.selectorType.setValue(C.DocumentType.Quotation);

    this.DepartmentService.getDepartments((department)  => {
      this.departments = department;
      if(this.departments){
        this.selectorDepartment.setValue(this.departments[0].code);
      }
    });
  }

  ngOnChanges() {
    if (this.isNew) {
      this.showModal();
    } else {
      this.closeModal();
    }
  }

  selectType() {
    let value = this.selectorType.value;

    if(value) {
      this.selectType();
    }
  }

  showModal(): void{
    if(this.isNew){
      this.triggerBtn("btn-show-modal-flow");
    }else{
      this.triggerBtn("btn-close-modal-flow");
    }
  }

  closeModal() {
    this.triggerBtn("btn-close-modal-flow");
    this.onCloseModal.emit();
    // this.cleanUserForm();
  }

  private triggerBtn(btnId : string) {
    let btn = document.getElementById(btnId);

    if (btn) {
      btn.click();
    }
  }
}
