import { Injectable, Type } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from "../base-http";
import { C } from "src/interfaces/constants";
import { Device, parseDevice, parseDevices } from "src/interfaces/catalog/Device";
import { User } from "src/interfaces/catalog/User";
import { dataBody } from "src/interfaces/catalog/dataBody";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private service : service;
  private urlExtension : string = "user";

  constructor(private http : HttpClient) {
    this.service = new service(C.urls.flowControl, http);
  }

  public getUser(fn: (res: User[]) => void) {
    this.service.getRequest(
      this.urlExtension,
      res => fn( res.data as User[] )
    );
  }

  public authUser(userName: string, password: string, fn: (res: dataBody) => void) {
    this.service.postRequest(
      this.urlExtension,
      { userName, password },
      fn
    );
  }

  public setUser(user: User, fn: (res: dataBody) => void) {
    this.service.postRequest(
      this.concatUrl("new"),
      {
        "employeeId": user.id,
        "userName": user.userName,
        "password": user.password,
        "type": user.userType
      },
      fn
    );
  }

  private concatUrl(ext: string) : string{
    return this.urlExtension + "/" + ext;
  }
}