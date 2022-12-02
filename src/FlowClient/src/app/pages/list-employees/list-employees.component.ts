import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/requests/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';
import { ActivatedRoute, Router } from '@angular/router';
import { C } from "src/interfaces/constants";
import { ControlContainer } from '@angular/forms';
import { Shift } from 'src/interfaces/catalog/Shift';
import { Position } from 'src/interfaces/catalog/Position';
import { User } from 'src/interfaces/catalog/User';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device } from 'src/interfaces/catalog/Device';

@Component({
  selector: 'app-list-employees',
  templateUrl: './list-employees.component.html',
})
export class ListEmployeesComponent implements OnInit {
  shifts?: Shift[];
  positions?: Position[];
  employees: Employee[] | undefined;
  selectedEmployee: Employee | undefined;
  modalEmployee?: Employee;
  devices? : Device[];
  deviceModal? : Device;
  deviceView? : Device;


  constructor(
    private service : EmployeeService,
    private _Activatedroute: ActivatedRoute,
    private services : DeviceService,
    private hubService : DeviceHubService,
    ) {
  }

  ngOnInit(): void {
    this.service.getEmployees(employees => {
      this.employees = employees;
    })

    this.services.getDevices(devices => {
      this.devices = devices;
      this.sortDevices();
    });

    this.hubService.setSubMonitor((device:Device) => this.addDevice(device));

    // this._Activatedroute.paramMap.subscribe(params => {
    //   let id = +(params.get('id') as string);
    //   this.service.getEmployee(id, employee => {
    //     this.employee = employee;
    //     this.imgEmployee = C.urls.accessControl + 'Employee/image/' + id.toString();
    //   })
    // })
  }

  selectDevice(device: Device) {
    this.deviceView = device;

    this.removeFromGroups();
    this.hubService.addToGroup(this.deviceView.name);


  }
  private removeFromGroups() {

    if (this.devices) {
      this.devices.forEach(d => {
        this.hubService.removeFromGroup(d.name);
      });
    }

  }

  selectEmployee(employee: Employee): void {
    this.selectedEmployee = employee;
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
  showModal(isNew: boolean = false): void{
    if(isNew){
      this.modalEmployee = {
        accessLevels: [],
        id: undefined,
        name: '',
        lastName: '',
        position: undefined,
        shift: undefined,
        image: '',
        status: undefined,
      }
    }else{
      this.modalEmployee = this.selectedEmployee;
    }
  }

  registerHint(){
    if(this.deviceView && this.selectedEmployee){
      this.services.setEmployeeHint(this.deviceView, this.selectedEmployee, res => {
        console.log(res);
      })
    }
  }


  closeModal(): void {
    this.modalEmployee = undefined;
  }
}
