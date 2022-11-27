import { Injectable, Type } from "@angular/core";
import { combineLatest, Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseHttp as service } from "../base-http";
import { C } from "src/interfaces/constants";
import { dataBody } from "src/interfaces/catalog/dataBody";
import { Shift } from "src/interfaces/catalog/Shift";
import { Position } from "src/interfaces/catalog/Position";

@Injectable({
  providedIn: 'root'
})

export class CatalogService {
  private service: service;
  private urlExtension : string = "catalog";

  constructor(private http: HttpClient){
    this.service = new service(C.urls.accessControl, http);
  }

  public getCatalog(fn: (res: Shift[], res2: Position[]) => void) {
    this.service.getRequest(
      this.concatUrl("assets"),
        res => {
          let shift : Shift[] = res.data as Shift[];
          let position : Position[] = res.data2 as Position[];
          fn(shift, position);
        }
    )
  }

  private concatUrl(ext: string) : string{
    return this.urlExtension + "/" + ext;
  }
}
