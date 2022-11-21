import { Component, OnInit, ElementRef } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device } from 'src/interfaces/catalog/Device';
import { EmployeeService } from 'src/app/services/requests/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';

@Component({
  selector: 'app-dashboard-principal',
  templateUrl: './dashboard-principal.component.html',
})
export class DashboardPrincipalComponent implements OnInit {

  devices? : Device[];

  employees? : Employee[];

  constructor(private elementRef: ElementRef, private service : DeviceService, private hubService : DeviceHubService, private services : EmployeeService) {
  }

  ngOnInit(): void {

    this.service.getDevices(devices => {
      this.devices = devices;
      this.sortDevices();
    });

    this.hubService.setSubMonitor(this.addDevice);

    this.services.getEmployees(employees => {
      this.employees = employees;

    });
    
    // var s = document.createElement("script");
    // s.type = "text/javascript";
    // s.src = "../assets/js/main.js";
    // this.elementRef.nativeElement.appendChild(s);
  }

  
  private addDevice(device: Device) {
    if(this.devices) {
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
        let dt1 : Date = a.last_update;
        let dt2 : Date = b.last_update;

        let diff = (dt2.getTime() - dt1.getTime());

        return diff;
      })
    }
  }

}
