import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Manufacturer } from '../home/models/manufacturer.model';

@Injectable({
  providedIn: 'root'
})
export class ManufacturersService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  public getManufacturers(): Observable<Manufacturer[]> {
    return this.http.get<Manufacturer[]>(this.baseUrl + 'manufacturers');
  }

  public create(name: string): Observable<any> {

    const body = {
      name: name
    }

    return this.http.post(this.baseUrl + 'manufacturers/create', body);
  }

  public delete(id: number): Observable<any> {

    return this.http.delete(this.baseUrl + `manufacturers/${id}/delete`);
  }
}
