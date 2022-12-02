import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Flow } from 'src/interfaces/api/Api';
import { Device } from 'src/interfaces/catalog/Device';
import { FlowService } from '../../../services/requests/flow.service';
import { DeviceService as deviceService } from '../../../services/requests/device.service';
import { FormControl } from '@angular/forms';
import { DataBody } from 'src/interfaces/catalog/DataBody';
import { C } from 'src/interfaces/constants';

@Component({
  selector: 'flow-modal',
  templateUrl: './modal-flow.component.html',
})
export class ModalFlowComponent implements OnInit {
  devices?: Device[];
  selector: FormControl = new FormControl();
  flowObj?: Flow;
  @Input() flow? : string;
  @Output() onDeviceAction = new EventEmitter<Device>();

  constructor(
    private devService : deviceService,
    private flowService: FlowService
  ) {
  }

  ngOnInit(): void {
    this.flowService.getFlows(flows => {
      this.flowObj = flows.find(x => x.name == this.flow)
    });

    this.devService.getDevices(devs => {
      this.devices = devs;
      if(this.devices && this.devices.length > 0) {
        this.selector.setValue(this.devices[0].id);
      }
    });
  }

  ngOnChanges() {
    if (this.flow) {
      this.showModal();
    }
  }

  setFlow() {
    if(this.flowObj) {
      let flowId: number = this.flowObj.id ;
      let deviceId : number = +this.selector.value

      this.flowService.setDevFlow(deviceId, flowId, (res : DataBody) => {

        if (res.status == C.keyword.OK) {
          this.devService.getDevice(deviceId, d => {
            this.onDeviceAction.emit(d);
            this.closeModal();
          });
        }

      });
    }
  }

  showModal() {
    this.triggerBtn("btn-show-modal");
  }

  closeModal() {
    this.triggerBtn("btn-close-modal");
  }

  private triggerBtn(btnId : string) {
    let btn = document.getElementById(btnId);

    if (btn) {
      btn.click();
    }

  }

}
