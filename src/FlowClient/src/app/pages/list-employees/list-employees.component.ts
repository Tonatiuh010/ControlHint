import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';
import { ActivatedRoute, Router } from '@angular/router';
import { C } from "src/interfaces/constants";
import { ControlContainer } from '@angular/forms';

@Component({
  selector: 'app-list-employees',
  templateUrl: './list-employees.component.html',
})
export class ListEmployeesComponent implements OnInit {
  employees: Employee[] | undefined;
  selectedEmployee: Employee | undefined;

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
}
