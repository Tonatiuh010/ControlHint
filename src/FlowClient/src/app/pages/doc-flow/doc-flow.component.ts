import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-doc-flow',
  templateUrl: './doc-flow.component.html',
})
export class DocFlowComponent implements OnInit {
  showModalApproved?: boolean = false;
  showModalFlow?: boolean = false;

  constructor() { }

  ngOnInit(): void {
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
