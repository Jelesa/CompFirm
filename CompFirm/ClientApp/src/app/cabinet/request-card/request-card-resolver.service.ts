import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { RequestCard } from '../../shared/models/request-card.model';
import { RequestsService } from "../../shared/requests.service";

@Injectable({
  providedIn: 'root'
})
export class RequestCardResolver implements Resolve<RequestCard> {

  constructor(private requestsService: RequestsService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<RequestCard> {
    let id = +route.paramMap.get('id');

    return this.requestsService.getRequestCard(id);
  }
}
