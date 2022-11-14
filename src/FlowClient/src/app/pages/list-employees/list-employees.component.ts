import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';

@Component({
  selector: 'app-list-employees',
  templateUrl: './list-employees.component.html',
})
export class ListEmployeesComponent implements OnInit {

  constructor(private service : EmployeeService) {
    console.log("No jala tu wea");
  }

  ngOnInit(): void {
    this.service.getEmployees(employees => {
      console.log(employees);
    })
  }

}
