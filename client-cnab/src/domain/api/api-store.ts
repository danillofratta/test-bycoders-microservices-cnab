import { Injectable } from '@angular/core';
import { ApiBase } from './api-base';
import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { GetAllStoresResponse } from '../dto/store/get-list/get-all-store-response';


@Injectable({
    providedIn: 'root'
})
export class ApiStore extends ApiBase {
   
  constructor(
    protected override http: HttpClient,    
    protected override router: Router
  ) {
    super(http, router);

    this._baseurl = environment.ApiUrl;

    if (!environment.production)
      this._endpoint = 'api/v1/stores/';
    else
      this._endpoint = 'api/v1/stores/';
  }
  
  async GetListAll(): Promise<Observable<GetAllStoresResponse[]>> {
    return this._http.get<GetAllStoresResponse[]>(`${this._baseurl + this._endpoint }`);
  }
}
