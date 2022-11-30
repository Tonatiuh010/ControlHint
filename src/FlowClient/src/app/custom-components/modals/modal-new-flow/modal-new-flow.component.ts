import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-modal-new-flow',
  templateUrl: './modal-new-flow.component.html'
})
export class ModalNewFlowComponent implements OnInit, OnChanges {
  @Input() isNew?: boolean;
  @Output() onCloseModal = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
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
