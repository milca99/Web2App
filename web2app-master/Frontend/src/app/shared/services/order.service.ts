import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Article } from '../models/article';
import { CancelOrder, CreateOrder } from '../models/order';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
newOrder!: CreateOrder;
baseUrl = environment.baseUrl;
token: any;

constructor(private http: HttpClient,
            private authService: AuthService) { }

getAllOrders(){
  return this.http.get(this.baseUrl + '/api/orders', this.getHttpHeader());
}

getOrderHistory(id: number){
  return this.http.get(this.baseUrl + '/api/orders/history/' + id, this.getHttpHeader());
}

getActiveOrder(id: number){
  return this.http.get(this.baseUrl + '/api/orders/active/' + id, this.getHttpHeader());
}

createOrder(order: CreateOrder){
  
  this.token = localStorage.getItem('token');
  order.userId = this.authService.getUserId(this.token);
  return this.http.post(this.baseUrl + '/api/orders/create',order ,this.getHttpHeader());
}

cancelOrder(cancelOrder: CancelOrder){
  return this.http.patch(this.baseUrl + '/api/orders/cancel', cancelOrder ,this.getHttpHeader());
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
