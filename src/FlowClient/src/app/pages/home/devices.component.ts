import { Component, OnInit, ElementRef, Output, EventEmitter } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device, parseDevice } from 'src/interfaces/catalog/Device';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {


  constructor(private elementRef: ElementRef, private service : DeviceService, private hubService : DeviceHubService) {

  }

  ngOnInit(): void {

  }

}
