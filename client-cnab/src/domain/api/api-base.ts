import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ApiBase {

  protected _baseurl = "";

  protected _endpoint = "";
 
  protected _http: HttpClient;
  protected _router: Router

  constructor(
    protected http: HttpClient,
    protected router: Router
  ) {
    this._http = http;
    this._router = router; 
  }
}
