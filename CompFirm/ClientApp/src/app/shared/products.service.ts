import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PriceInterval } from '../home/models/price-interval.model';
import { ProductInfo } from '../home/models/productInfo.model';
import { ProductsFilter } from '../home/models/productsFilter.model';
import { BaseSearchFilter } from './models/base-search-filter.model';
import { ProductShortInfo } from './models/product-short-info.model';
import { SearchResult } from './models/search-result.model';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  public getProducts(filter: ProductsFilter = null): Observable<SearchResult<ProductInfo>> {

    const filterStr = this.getFilterStr(filter);

    return this.http.get<SearchResult<ProductInfo>>(this.baseUrl + 'products/search?' + filterStr);
  }

  public getProduct(id: number): Observable<ProductInfo> {
    return this.http.get<ProductInfo>(this.baseUrl + `products/${id}`);
  }

  public getPrices(): Observable<PriceInterval> {
    return this.http.get<PriceInterval>(this.baseUrl + 'products/price-interval');
  }

  public deleteReturnedProduct(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `products/delete-returned-product/${id}`);
  }

  public getProductsShortInfo(filter: BaseSearchFilter): Observable<ProductShortInfo[]> {

    let paramArr: string[] = [];

    paramArr.push(`limit=${filter.limit || 20}`);
    paramArr.push(`offset=${filter.offset || 0}`);

    const filterStr = paramArr.join('&');

    return this.http.get<ProductShortInfo[]>(this.baseUrl + 'products/?' + filterStr);
  }

  public createProduct(product: ProductInfo): Observable<any> {
    product.price = +product.price;

    return this.http.post(this.baseUrl + `products/create`, product);
  }

  public updateProduct(product: ProductInfo): Observable<any> {

    product.price = +product.price;

    return this.http.put(this.baseUrl + `products/${product.productId}/update`, product);
  }

  public deleteProduct(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `products/${id}/delete`);
  }

  private getFilterStr(filter: ProductsFilter): string {

    if (!filter) {
      return '';
    }

    let paramArr: string[] = [];

    if (filter.groupId !== -999) {
      paramArr.push(`groupId=${filter.groupId}`);
    }

    if (!!filter.name) {
      paramArr.push(`name=${filter.name}`);
    }

    if (!!filter.productType) {
      paramArr.push(`productType=${filter.productType}`);
    }

    paramArr.push(`minPrice=${filter.minPrice || 0}`);
    paramArr.push(`maxPrice=${filter.maxPrice || 10000000}`);

    paramArr.push(`limit=${filter.limit || 20}`);
    paramArr.push(`offset=${filter.offset || 0}`);

    return paramArr.join('&');
  }
}
