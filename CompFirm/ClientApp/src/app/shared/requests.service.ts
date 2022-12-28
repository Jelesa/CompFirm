import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RequestCard } from './models/request-card.model';
import { ItemCart } from '../home/models/itemCart.model';
import { CartItem } from './models/cart-item.model';
import { SearchResult } from "./models/search-result.model";
import { RequestsFilter } from './models/requests-filter.model';
import { BaseReferenceBook } from "./models/base-reference-book.model";

@Injectable({
  providedIn: 'root'
})
export class RequestsService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  public addItemToCart(item: ItemCart) {
    return this.http.put(this.baseUrl + 'requests/addToCart', item);
  }

  public getCartItems(): Observable<CartItem[]> {
    return this.http.get<CartItem[]>(this.baseUrl + 'requests/cart');
  }

  public confirmRequest() {
    return this.http.put(this.baseUrl + 'requests/confirm', null);
  }

  public getRequests(filter: RequestsFilter): Observable<SearchResult<RequestCard>> {

    let paramArr: string[] = [];

    if (filter.adminSearch || filter.adminSearch !== false) {
      paramArr.push(`adminSearch=${filter.adminSearch}`);
    }

    if (!!filter.searchString) {
      paramArr.push(`searchString=${filter.searchString}`);
    }

    if (filter.status !== -1) {
      paramArr.push(`status=${filter.status}`);
    }

    paramArr.push(`limit=${filter.limit || 20}`);
    paramArr.push(`offset=${filter.offset || 0}`);

    const filtStr = paramArr.join('&');

    return this.http.get<SearchResult<RequestCard>>(this.baseUrl + 'requests?' + filtStr);
  }

  public getRequestCard(requestId: number): Observable<RequestCard> {
    return this.http.get<RequestCard>(this.baseUrl + 'requests/' + requestId);
  }

  public cancelRequest(requestId: number) {
    return this.http.put(this.baseUrl + `requests/${requestId}/cancel`, null);
  }

  public statusUpdate(requestId: string, status: string) {
    let body = {
      requestId: +requestId,
      statusName: status
    };

    return this.http.put(this.baseUrl + `requests/status-update`, body);
  }

  public getStatuses(): Observable<BaseReferenceBook[]> {
    return this.http.get<BaseReferenceBook[]>(this.baseUrl + `requests/statuses`);
  }
}
