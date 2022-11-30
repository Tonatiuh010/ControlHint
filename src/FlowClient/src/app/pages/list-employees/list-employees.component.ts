import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/requests/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';
import { ActivatedRoute, Router } from '@angular/router';
import { C } from "src/interfaces/constants";
import { ControlContainer } from '@angular/forms';
import { Shift } from 'src/interfaces/catalog/Shift';
import { Position } from 'src/interfaces/catalog/Position';
import { User } from 'src/interfaces/catalog/User';

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

  constructor(
    private service : EmployeeService,
    private _Activatedroute: ActivatedRoute
    ) {
  }

  ngOnInit(): void {
    this.service.getEmployees(employees => {
      this.employees = employees;
    })

    // this._Activatedroute.paramMap.subscribe(params => {
    //   let id = +(params.get('id') as string);
    //   this.service.getEmployee(id, employee => {
    //     this.employee = employee;
    //     this.imgEmployee = C.urls.accessControl + 'Employee/image/' + id.toString();
    //   })
    // })
  }

  selectEmployee(employee: Employee): void {
    this.selectedEmployee = employee;
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



  closeModal(): void {
    this.modalEmployee = undefined;
  }
}
