import { Component, OnInit, ElementRef } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device } from 'src/interfaces/catalog/Device';
import { EmployeeService } from 'src/app/services/requests/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';
import { Check } from 'src/interfaces/catalog/Check';

@Component({
  selector: 'app-dashboard-principal',
  templateUrl: './dashboard-principal.component.html',
})
export class DashboardPrincipalComponent implements OnInit {

  devices? : Device[];
  deviceModal? : Device;
  Checks? : Check;
  name? : Employee;
  job? : Employee;
  shift? : Employee;
  shiftend? : Employee;
  logPanel: boolean = false;
  deviceView? : Device;

  constructor(private elementRef: ElementRef, private service : DeviceService, private hubService : DeviceHubService, private services : EmployeeService) {
  }

  ngOnInit(): void {

    this.service.getDevices(devices => {
      this.devices = devices;
      this.sortDevices();
    });

    this.hubService.setSubMonitor((device:Device) => this.addDevice(device));
    

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
  selectDevice(device: Device) {
    this.deviceView = device;

    this.removeFromGroups();
    this.hubService.addToGroup(this.deviceView.name);
  
    this.hubService.setSubSignal((...args: any[]) =>{
      console.log(args[0].hintConfig.employee)
      this.name = args[0].hintConfig.employee.name;
      this.job = args[0].hintConfig.employee.job.name;
      this.shift = args[0].hintConfig.employee.shift.inTime;
      this.shiftend = args[0].hintConfig.employee.shift.outTime;

      this.hubService.setBroadcast((...args: any[]) =>{
      })
    })
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
