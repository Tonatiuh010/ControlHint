import { Component, OnInit, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';

@Component({
  selector: 'observing-device',
  templateUrl: 'observing-device.component.html',
})
export class ObservingComponent implements OnInit,  OnChanges {
  @Input() name? : string;
  msgs: string[] = [];

  constructor(
    private hubService : DeviceHubService,
    private serviceDevice : DeviceService
  ) {

  }

  ngOnInit(): void {
    this.prepareObserving();
  }

  ngOnChanges() {
    this.prepareObserving();
  }

  prepareObserving() {
    this.removeFromGroups();
    if(this.name) {
      this.hubService.addToGroup(this.name);
      this.hubService.setSubInfo((type: string, msg: string ) => {
        this.setLog(type, msg)
      });
    }
  }

  private removeFromGroups() {
    this.serviceDevice.getDevices(devs => {
      devs.forEach(d => {
        this.hubService.removeFromGroup(d.name);
      });
    })
  }

  private setLog(type: string, msg: string) {
    if(this.msgs.length < 10) {
      this.msgs.push(msg);
    } else {
      let _temp: string[] = [msg];
      this.msgs.splice(1, 9).forEach(msg => _temp.push(msg));
      this.msgs = _temp;
    }
  }

}
