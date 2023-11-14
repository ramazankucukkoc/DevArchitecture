import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../environments/environment'
import { WarehouseInformation } from '../models/WarehouseInformation';


@Injectable({
  providedIn: 'root'
})
export class WarehouseInformationService {

  constructor(private httpClient: HttpClient) { }


   getwarehouseInformationList(): Observable<WarehouseInformation[]> {
    return this.httpClient.get<WarehouseInformation[]>(environment.getApiUrl + "/warehouseInformations/");
  }

  getwarehouseInformationById(id: number): Observable<WarehouseInformation> {
     return this.httpClient.get<WarehouseInformation>(environment.getApiUrl + `/warehouseInformations/${id}`);
   }
  
  addwarehouseInformation(warehouseInformation: WarehouseInformation): Observable<any> {

    var result = this.httpClient.post(environment.getApiUrl + "/warehouseInformations/", warehouseInformation, { responseType: 'text' });
    return result;
  }

    updatewarehouseInformation(warehouseInformation:WarehouseInformation):Observable<any> {
     var result = this.httpClient.put(environment.getApiUrl + "/warehouseInformations/", warehouseInformation, { responseType: 'text' });
      return result;
    }

   deletewarehouseInformation(id: number) {
     return this.httpClient.request('delete', environment.getApiUrl + `/warehouseInformations/${id}`);
   }
}
