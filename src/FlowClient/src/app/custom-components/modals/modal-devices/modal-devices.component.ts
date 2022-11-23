import { Component, OnInit, ElementRef, Output, EventEmitter } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device, parseDevice } from 'src/interfaces/catalog/Device';

@Component({
  selector: 'modal-devices',
  templateUrl: './modal-devices.component.html',
})
export class ModalDevicesComponent implements OnInit {
  logPanel: boolean = false;
  devices? : Device[];
  deviceView? : Device;
  deviceModal? : Device;

  constructor(private elementRef: ElementRef, private service : DeviceService, private hubService : DeviceHubService) {
  }

  ngOnInit(): void {
    this.service.getDevices(devices => {
      this.devices = devices;
      this.sortDevices();
    });

    this.hubService.setSubMonitor( (device: Device) => this.addDevice(device));

  }
  ngOnChanges() {
    this.showModal();
  }

  selectDevice(device: Device) {
    this.deviceView = device;

    this.removeFromGroups();
    this.hubService.addToGroup(this.deviceView.name);
    this.hubService.setSubInfo((type: string, msg: string ) => {
      this.setLog(type, msg)
    });
  }

  showModals(device: Device) {
    this.deviceModal = device;
  }

  addDevice(device: Device) {
    if(this.devices) {

      device = parseDevice(device);

      let index = this.devices.findIndex(x => x.name == device.name);
      if (index != -1) {
        this.devices[index] = device;
      } else {
        this.devices.push(device);
      }

      this.sortDevices();

    }
  }

  private sortDevices() {
    if(this.devices) {
      this.devices.sort((a, b) =>  {
        let dt1 : Date = new Date(a.last_update);
        let dt2 : Date = new Date(b.last_update);

        let diff = (dt2.getTime() - dt1.getTime());

        return diff;
      })
    }
  }

  private removeFromGroups() {

    if (this.devices) {
      this.devices.forEach(d => {
        this.hubService.removeFromGroup(d.name);
      });
    }

    this.clearComponent();
  }

  private setLog(type: string, msg: string) {
    let element = this.getLogContainer();

    if (element) {
      let now = new Date(Date.now());
      let p: HTMLParagraphElement = document.createElement('p');
      p.innerHTML = `[${now.toLocaleString()}] ${msg}`;
      element?.appendChild(p);
    }

  }

  private getLogContainer() : HTMLElement | null {
    return document.getElementById("device-log");
  }

  private clearComponent() {
    let element = this.getLogContainer();

    if (element) {
      element.innerHTML = "";
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
