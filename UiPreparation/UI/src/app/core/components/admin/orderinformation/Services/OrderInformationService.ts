import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../environments/environment'
import { OrderInformation } from '../models/OrderInformation';


@Injectable({
  providedIn: 'root'
})
export class OrderInformationService {

  constructor(private httpClient: HttpClient) { }

   getorderInformationList(): Observable<OrderInformation[]> {
     return this.httpClient.get<OrderInformation[]>(environment.getApiUrl + "/orderInformations/");
   }

   getorderInformationById(id: number): Observable<OrderInformation> {
  return this.httpClient.get<OrderInformation>(environment.getApiUrl + `/orderInformations/${id}`);
 }
  addorderInformation(orderInformation: OrderInformation): Observable<any> {
    var result = this.httpClient.post(environment.getApiUrl + "/orderInformations/", orderInformation, { responseType: 'text' });
    return result;
  }

   updateorderInformation(orderInformation:OrderInformation):Observable<any> {
     var result = this.httpClient.put(environment.getApiUrl + "/orderInformations/", orderInformation, { responseType: 'text' });
     return result;
   }

   deleteorderInformation(id: number) {
     return this.httpClient.request('delete', environment.getApiUrl + `/orderInformations/${id}`);
   }
}
