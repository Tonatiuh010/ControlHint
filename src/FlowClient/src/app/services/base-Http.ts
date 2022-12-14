import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { C } from "src/interfaces/constants";
import { DataBody } from "src/interfaces/catalog/DataBody";

export class BaseHttp {
  url : string;
  constructor(url : string, private http: HttpClient){
    this.url = url;
  }

  public getRequest(urlExtension: string = '', fn: (res: DataBody) => void){
    try {
      let obs = this.http.get<DataBody>(this.url + urlExtension);

      this.responseBlock(
        obs,
        fn,
        () => {
          throw 'Error reading response. ';
        }
      );
    } catch {
      throw 'Exception getting data body. ';
    }
  }

  public postRequest(urlExtension: string = '', body: any, fn: (res: any) => void ) {
    try {
      let obs = this.http.post<any>(this.url + urlExtension, body);

      this.responseBlock(
        obs,
        fn,
        () => {
          throw 'Error reading response. ';
        }
      );
    } catch {
      throw 'Exception getting data body. ';
    }
  }

  private responseBlock(response: Observable<DataBody>, onComplete: (res: DataBody) => void, onError: (err: any) => void) {
    let ref = this;

    response.subscribe({
      next(res: DataBody){
        if(ref.isValidResponse(res))
          onComplete(res);
        else
          onError('Error from request');
      },
      error(err){
        onError(err);
      }
    })
  }

  public isValidResponse(data: any) : boolean {
    return data.status == C.keyword.OK && data.message == C.keyword.COMPLETE;
  }
}
