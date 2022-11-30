import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ApproverService } from "src/app/services/requests/approver.service";
import { Approver, ApproverDocument } from "src/interfaces/document/Approver";

@Component({
  selector: 'app-modal-approved',
  templateUrl: './modal-approved.component.html',
})
export class ModalApprovedComponent implements OnInit, OnChanges {
  approvers?: Approver[];

  selectorApprover: FormControl = new FormControl();
  selectorDepartment: FormControl = new FormControl();
  selectorType: FormControl = new FormControl();
  approverDocuments?: ApproverDocument[];

  approver?: Approver;

  @Input() isNew?: boolean;
  @Output() onCloseModal = new EventEmitter();

  constructor(
    private ApproverService : ApproverService,
  ) { }

  ngOnInit(): void {
    this.ApproverService.getApprovers((approver) => {
      this.approvers = approver;
      if(this.approvers){
        this.selectorApprover.setValue(this.approvers[0].id);
      }
    })
  }

  ngOnChanges() {
    if (this.isNew) {
      this.showModal();
    } else {
      this.closeModal();
    }
  }


  showModal(): void{
    if(this.isNew){
      this.triggerBtn("btn-show-modal");
    }else{
      this.triggerBtn("btn-close-modal");
    }
  }

  closeModal() {
    this.triggerBtn("btn-close-modal");
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
