import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Characteristic } from './models/characteristics.model';

@Injectable({
  providedIn: 'root'
})
export class CharacteristicsService {

  constructor(private http: HttpClient, @Inject('API_URL') private baseUrl: string) { }

  public getAll(): Observable<Characteristic[]> {
    return this.http.get<Characteristic[]>(this.baseUrl + 'characteristics');
  }

  public create(name: string, unit: string): Observable<any> {

    const body = {
      name: name,
      unit: unit
    }

    return this.http.post(this.baseUrl + 'characteristics/create', body);
  }

  public delete(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `characteristics/${id}/delete`);
  }
}
