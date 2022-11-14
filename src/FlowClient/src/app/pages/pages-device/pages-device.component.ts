import { Component, OnInit,ElementRef } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device } from 'src/interfaces/catalog/Device';

@Component({
  selector: 'app-pages-device',
  templateUrl: './pages-device.component.html',
})
export class PagesDeviceComponent implements OnInit {
  devices? : Device[];

  constructor(private elementRef: ElementRef, private service : DeviceService, private hubService : DeviceHubService) {
  }

  ngOnInit(): void {
    this.service.getDevices(devices => {
      this.devices = devices;
    });

    this.hubService.setSubMonitor((device : any) => {
      console.log(device);
    });
    // var s = document.createElement("script");
    // s.type = "text/javascript";
    // s.src = "../assets/js/main.js";
    // this.elementRef.nativeElement.appendChild(s);
  }

  onDevice(device: Device) {

  }

}
