import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UpdateUser } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) { }

  verify(id: number){
    return this.http.patch(this.baseUrl + '/api/users/verify/' + id, null , this.getHttpHeader());
  }

  deny(id: number){
    return this.http.patch(this.baseUrl + '/api/users/deny/' + id, null, this.getHttpHeader());
  }

  getSeller(){
    return this.http.get(this.baseUrl + '/api/users/sellers', this.getHttpHeader());
  }

  getUserDetails(id: number){
    return this.http.get(this.baseUrl + '/api/users/' + id, this.getHttpHeader());
  }

  update(user: UpdateUser){
    return this.http.patch(this.baseUrl + '/api/users/update', user, this.getHttpHeader());
  }

  getHttpHeaderNoToken(): { headers: HttpHeaders; }{
    const httpOptions = {
      headers: new HttpHeaders({
        Accept: "application/json"
      })
    };
    return httpOptions;
  }
  getHttpHeader(): { headers: HttpHeaders; }{
    const httpOptions = {
      headers: new HttpHeaders({
        Accept: "application/json",
        Authorization: 'Bearer '+ localStorage.getItem('token')
      })
    };
    return httpOptions;
  }
}
