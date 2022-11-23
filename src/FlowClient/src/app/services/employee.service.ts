import { Injectable, Type } from "@angular/core";
import { combineLatest, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service  } from "./base-Http";
import { C } from "src/interfaces/constants";
import { Employee } from "src/interfaces/catalog/Employee";

@Injectable({
  providedIn: 'root'
})

export class EmployeeService {
  private service : service;
  private urlExtension : string = "employee";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.accessControl, http);
  }

  public getEmployees(fn: (res: Employee[]) => void) {
    this.service.getRequest(
      this.urlExtension,
      res => {
        let employees : Employee[] = res.data as Employee[];
        employees.map(e => {
          e.image = (C.urls.accessControl + "employee/image/" + e.id);
          return e;
        });
        fn(employees);
      }
    );
  }

  public getEmployee(id: number, fn: (res: Employee) => void) {
    this.service.getRequest(
      this.concatUrl(id.toString()),
      res => fn(res.data as Employee)
    );
  }

  //SE PODRIA AGREGAR UN FILTRADO DE LOS EMPLEADOS POR DEPARTAMENTOS
  //CONSEGUIR EL PUESTO DEL TRABAJADOR

private concatUrl(ext: string) : string{
  return this.urlExtension + "/" + ext;
}
}
