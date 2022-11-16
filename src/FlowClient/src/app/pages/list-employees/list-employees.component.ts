import { Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/services/employee.service';
import { Employee } from 'src/interfaces/catalog/Employee';

@Component({
  selector: 'app-list-employees',
  templateUrl: './list-employees.component.html',
  styleUrls: ['./list-employees.component.css']
})
export class ListEmployeesComponent implements OnInit {
  employees: Employee[] | undefined;

  constructor(private service : EmployeeService) {
  }

  ngOnInit(): void {
    this.service.getEmployees(employees => {
      this.employees = employees;
    })
  }

}
