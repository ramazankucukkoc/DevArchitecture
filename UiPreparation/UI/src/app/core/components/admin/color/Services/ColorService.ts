import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../environments/environment'
import { Color } from '../models/color';


@Injectable({
  providedIn: 'root'
})
export class ColorService {

  constructor(private httpClient: HttpClient) { }

   getColorList(): Observable<Color[]> {
     return this.httpClient.get<Color[]>(environment.getApiUrl + "/colors/");
   }

   getColorById(id: number): Observable<Color> {
     return this.httpClient.get<Color>(environment.getApiUrl + `/colors/${id}`);
  }

  addColor(color: Color): Observable<any> {

    var result = this.httpClient.post(environment.getApiUrl + "/colors/", color, { responseType: 'text' });
    return result;
  }

   updateColor(color:Color):Observable<any> {
     var result = this.httpClient.put(environment.getApiUrl + "/colors/", color, { responseType: 'text' });
     return result;
   }

   deleteColor(id: number) {
     return this.httpClient.request('delete', environment.getApiUrl + `/colors/${id}`);
   }
}
