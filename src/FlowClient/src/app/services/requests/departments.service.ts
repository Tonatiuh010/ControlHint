import { Injectable, Type } from "@angular/core";
import { combineLatest, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from './../base-http' ;
import { C } from "src/interfaces/constants";
import { Department } from "src/interfaces/catalog/Department";

@Injectable({
    providedIn: 'root'
})
export class DepartmentService{
    private service : service;
    private urlExtension : string = "departament";

    constructor(private http : HttpClient) {
        this.service = new service(C.urls.accessControl, http);
    }

    public getDepartments(fn: (res: Department[]) => void) {
        this.service.getRequest(
          this.urlExtension,
          res => fn(res.data as Department[])
        );
    }

    public getDepartment(id: number, fn: (res: Department) => void) {
        this.service.getRequest(
            this.concatUrl(id.toString()),
            res => fn(res.data as Department)
        );
    }

    private concatUrl(ext: string) : string{
        return this.urlExtension + "/" + ext;
    }



}
