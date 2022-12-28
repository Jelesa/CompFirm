import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SearchResult } from "../../shared/models/search-result.model";
import { ProductsMoving } from "./products-moving.model";
import { Observable } from 'rxjs';
import { ProductsMovingFilter } from './products-moving-filter.model';

@Injectable({
  providedIn: 'root'
})
export class ProductsMovingService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  getProductMoving(filter: ProductsMovingFilter): Observable<SearchResult<ProductsMoving>> {

    let paramArr: string[] = [];

    if (!!filter.dateFrom) {
      paramArr.push(`dateFrom=${filter.dateFrom}`);
    }

    if (!!filter.dateTo) {
      paramArr.push(`dateTo=${filter.dateTo}`);
    }

    if (!!filter.movingType) {
      paramArr.push(`movingType=${filter.movingType}`);
    }

    paramArr.push(`limit=${filter.limit}`);
    paramArr.push(`offset=${filter.offset}`);

    let paramStr = paramArr.join('&');

    console.log(filter);
    console.log(paramStr);

    return this.http.get<SearchResult<ProductsMoving>>(this.baseUrl + `products-moving?${paramStr}`);
  }

  create(movings: ProductsMoving[]): Observable<any> {

    return this.http.post(this.baseUrl + `products-moving`, movings);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `products-moving/${id}/delete`);
  }
}
