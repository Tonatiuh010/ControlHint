import { Component, OnInit, ElementRef, NgModule } from '@angular/core';
import { DeviceHubService } from 'src/app/services/hubs/device-hub.service';
import { DeviceService } from 'src/app/services/requests/device.service';
import { Device } from 'src/interfaces/catalog/Device';
import { EmployeeService } from 'src/app/services/requests/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';
import { Checks } from 'src/interfaces/catalog/Check'; 


@Component({
  selector: 'app-dashboard-principal',
  templateUrl: './dashboard-principal.component.html',
})
export class DashboardPrincipalComponent implements OnInit {

  devices? : Device[];
  deviceModal? : Device;
  Checks? : null;
  lunesin? : any;
  martesin? : any;
  miercolesin? : any;
  juevesin? : any;
  viernesin? : any;
  sabadoin? : any;
  domingoin? : any;
  lunesout? : any;
  martesout? : any;
  miercolesout? : any;
  juevesout? : any;
  viernesout? : any;
  sabadoout? : any;
  domingoout? : any;
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

      this.services.getEmployeeChecks(args[0].hintConfig.employee.id as number, checks => {
        
        this.Checks = checks
        this.name = checks.employee.name
        this.lastname = checks.employee.lastName
        this.job = checks.employee.job.name
        this.shiftin = checks.employee.shift.inTime
        this.shiftout = checks.employee.shift.outTime
        this.domingoin= checks.checks[0].in
        this.lunesin = checks.checks[1].in
        this.martesin= checks.checks[2].in
        this.miercolesin= checks.checks[3].in
        this.juevesin= checks.checks[4].in
        this.viernesin= checks.checks[5].in
        this.sabadoin= checks.checks[6].in
        this.domingoout= checks.checks[0].out
        this.lunesout = checks.checks[1].out
        this.martesout= checks.checks[2].out
        this.miercolesout= checks.checks[3].out
        this.juevesout= checks.checks[4].out
        this.viernesout= checks.checks[5].out
        this.sabadoout= checks.checks[6].out
        this.image = "https://accesscontrol9a.azurewebsites.net/api/employee/image/"+ args[0].hintConfig.employee.id.toString()
        console.log(this.image)

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
