import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ApiService {

  constructor(private http: HttpClient) {
  }

  AddPerson(route: string | 'person', body: any) {
    return this.http.post(`http://localhost:5000/${route}`, body, { observe: 'response' })
    .pipe(
      map((res: HttpResponse<any>) => {
        return { ...res.body };
      })
    )
  }
}