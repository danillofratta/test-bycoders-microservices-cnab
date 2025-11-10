import { Injectable } from '@angular/core';
import { ApiBase } from './api-base';
import { HttpClient, HttpEventType, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { TransactionGetListResponse } from '../dto/sale/get-list/transaction-get-list-response';


@Injectable({
    providedIn: 'root'
})
export class ApiUpload extends ApiBase {
   
  constructor(
    protected override http: HttpClient,    
    protected override router: Router
  ) {
    super(http, router);

    this._baseurl = environment.ApiUrl;

    if (!environment.production)
      this._endpoint = 'api/v1/transactions/';
    else
      this._endpoint = 'api/v1/transactions/';
  }
  
  async GetListAll(): Promise<Observable<TransactionGetListResponse[]>> {
    return this._http.get<TransactionGetListResponse[]>(`${this._baseurl + this._endpoint }`);
  }

  async Upload(data: FormData): Promise<Observable<any>> {
    return this._http.post<any>(`${this._baseurl + this._endpoint}`, data, {
      reportProgress: true,
      observe: 'events'
    });
  }
}
