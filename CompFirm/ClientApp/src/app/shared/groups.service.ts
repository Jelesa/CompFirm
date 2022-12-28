import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Group } from '../home/models/group.model';
import { SearchResult } from './models/search-result.model';

@Injectable({
  providedIn: 'root'
})
export class GroupsService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  public getMainGroups(): Observable<Group[]> {
    return this.http.get<Group[]>(this.baseUrl + 'groups');
  }

  public getChildGroups(id: number): Observable<Group[]> {
    return this.http.get<Group[]>(this.baseUrl + 'groups/' + id + '/search');
  }

  public getProductTypes(id: number): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'groups/' + id + '/product-types');
  }

  public getGroups(limit: number, offset: number): Observable<SearchResult<Group>> {
    return this.http.get<SearchResult<Group>>(this.baseUrl + `groups/all?limit=${limit}&offset=${offset}`);
  }

  public deleteGroup(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `groups/${id}/delete`);
  }

  public editGroup(group: Group): Observable<any> {
    return this.http.put(this.baseUrl + `groups/${group.id}/update`, group);
  }

  public createGroup(group: Group): Observable<any> {
    return this.http.post(this.baseUrl + `groups/create`, group);
  }

  public getGroupById(id: number): Observable<Group> {
    return this.http.get<Group>(this.baseUrl + `groups/${id}`);
  }
}
