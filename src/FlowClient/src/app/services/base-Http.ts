import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { C } from "src/interfaces/constants";
import { dataBody } from "src/interfaces/catalog/dataBody";

export class BaseHttp {
  url : string;
  constructor(url : string, private http: HttpClient){
    this.url = url;
  }

  public getRequest(urlExtension: string = '', fn: (res: dataBody) => void){
    try {
      let obs = this.http.get<dataBody>(this.url + urlExtension);

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

  private responseBlock(response: Observable<dataBody>, onComplete: (res: dataBody) => void, onError: (err: any) => void) {
    let ref = this;

    response.subscribe({
      next(res: dataBody){
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
