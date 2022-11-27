import { Injectable, Type } from "@angular/core";
import { combineLatest, concatMap, Observable, startWith } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from "../base-http";
import { C } from "src/interfaces/constants";
import { Employee } from "src/interfaces/catalog/Employee";
import { Subject } from "@microsoft/signalr";

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

  public setEmployee(employee: Employee,image: string | undefined,fn: (res: any) => void){
    this.service.postRequest(
      this.urlExtension,
      { image: image,
        id: employee.id,
        name: employee.name,
        lastName: employee.lastName,
        position: employee.position?.positionId,
        shift: employee.shift?.id
      },
      res => fn(res)
    );
  }


  //SE PODRIA AGREGAR UN FILTRADO DE LOS EMPLEADOS POR DEPARTAMENTOS
  //CONSEGUIR EL PUESTO DEL TRABAJADOR

private concatUrl(ext: string) : string{
  return this.urlExtension + "/" + ext;
}
}
