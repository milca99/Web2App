import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Article, NewArticle } from '../models/article';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  
  baseUrl = environment.baseUrl;
  token!: any;
  sellerId!: any;
  constructor(private http: HttpClient,
              private authService: AuthService) { }

  create(article: NewArticle, picture: File){
    this.token = localStorage.getItem('token');
    article.userId = this.authService.getUserId(this.token);
    const formData: FormData = new FormData();

    formData.append('name' , article.name);
    formData.append('price' , article.price.toString());
    formData.append('quantity',article.quantity.toString());
    formData.append('description',article.description);
    formData.append('picture', "../../../assets/pictures/" + picture.name);
    formData.append('userId',article.userId.toString());
    formData.append('file',picture);

    return this.http.post(this.baseUrl + '/api/articles/create', formData, this.getHttpHeader());
  }

  update(article: Article){
    const formData: FormData = new FormData();
    formData.append('id' , article.id.toString());
    formData.append('name' , article.name);
    formData.append('price' , article.price.toString());
    formData.append('quantity',article.quantity.toString());
    formData.append('description',article.description);
    formData.append('picture',article.picture);
    formData.append('userId',article.userId.toString());
    formData.append('file',article.file);
    return this.http.patch(this.baseUrl + '/api/articles/update', formData, this.getHttpHeader());
  }

  getArticalDetails(id: number){
    return this.http.get(this.baseUrl + '/api/articles/details/' + id, this.getHttpHeader());
  }

  getAllArticles(){
    return this.http.get(this.baseUrl + '/api/articles', this.getHttpHeader());
  }

  getSellerArticles(id: number){
    return this.http.get(this.baseUrl + '/api/articles/' + id, this.getHttpHeader());
  }

  deleteArticle(id: number){
    this.token = localStorage.getItem('token');
    this.sellerId = this.authService.getUserId(this.token);
    return this.http.delete(this.baseUrl + '/api/articles/delete/' + id + '/' + this.sellerId , this.getHttpHeader());
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
