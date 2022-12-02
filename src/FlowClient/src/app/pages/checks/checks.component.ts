import { Component, OnInit, ElementRef, NgModule } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device } from 'src/interfaces/catalog/Device';
import { EmployeeService } from 'src/app/services/requests/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';
import { Checks } from 'src/interfaces/catalog/Check';
import { C } from 'src/interfaces/constants';

@Component({
  selector: 'app-checks',
  templateUrl: './checks.component.html',
})
export class ChecksComponent implements OnInit {

  devices? : Device[];
  deviceModal? : Device;
  Checks? : null;
  image? : any;
  name? : Checks;
  lastname? : Checks;
  job? : Checks;
  shiftin? : Checks;
  shiftout? : Employee;
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
  }

  ngOnChange() {
    if (this.devices && this.devices.length > 0 ) {
      this.selectDevice(this.devices[0]);
    }
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

      this.services.getEmployeeChecks(args[0].hintConfig.employee.id as number, checks => {

        if (checks.checks.in != null) {
          let dt = new Date(checks.checks.in.checkDt)
          dt.toLocaleTimeString() 
        }

        if (checks.checks.out != null) {
          let dt = new Date(checks.checks.out.checkDt)
          dt.toLocaleTimeString() 
        }

        console.log(checks)
        this.Checks = checks
        this.name = checks.employee.name
        this.lastname = checks.employee.lastName
        this.job = checks.employee.job.alias
        this.shiftin = checks.employee.shift.inTime
        this.shiftout = checks.employee.shift.outTime
        this.Checks = checks.checks
        this.image =  C.urls.accessControl + "employee/image/"+ args[0].hintConfig.employee.id.toString();

      })
    })
  }
  private removeFromGroups() {

    if (this.devices) {
      this.devices.forEach(d => {
        this.hubService.removeFromGroup(d.name);
      });
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
