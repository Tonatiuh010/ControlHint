import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Flow } from 'src/interfaces/api/Api';
import { Device } from 'src/interfaces/catalog/Device';
import { FlowService as service } from '../../../services/requests/flow.service';
import { DeviceService as deviceService } from '../../../services/requests/device.service';
import { FormControl } from '@angular/forms';
import { dataBody } from 'src/interfaces/catalog/dataBody';
import { C } from 'src/interfaces/constants';

@Component({
  selector: 'modal-devices',
  templateUrl: './modal-devices.component.html',
})
export class ModalDevicesComponent implements OnInit {
  flows?: Flow[];
  selector: FormControl = new FormControl();
  @Input() device? : Device[];
  @Output() onDeviceAction = new EventEmitter<Device>();

  constructor( private service : service, private devService : deviceService) {
    service.getFlows(flows => this.flows = flows);
  }

  ngOnInit(): void {
  }

  ngOnChanges() {
    if (this.device) {
      this.showModal();
    }
  }

  setFlow() {
    if(this.device) {
      let flowId: number = +this.selector.value;
      let deviceId : number = this.device[0].id;

      this.service.setDevFlow(deviceId, flowId, (res : dataBody) => {

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
