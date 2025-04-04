import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { map, take } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';

@Injectable({
  //{  providedIn: 'root'}   injeçao de dependencia (outra forma)
  providedIn: 'root'
})

export class PalestranteService {

  baseURL = environment.apiURL + 'api/palestrante';

  //tokenHeader = new HttpHeaders({'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user') || '{}').token}`});
  //tokenHeader = new HttpHeaders({'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxMCIsInVuaXF1ZV9uYW1lIjoidHJ1amlsbG8iLCJuYmYiOjE3MzgzMjY3NTAsImV4cCI6MTczODQxMzE1MCwiaWF0IjoxNzM4MzI2NzUwfQ.XC1JeTPyGLVYtOUjXWWpmB5dseqyp6FQEtardvXtWfci_KlU0s4Q_gisLJcPxT4R_bVRC5sYHv0bwqZjpKzxLg'});
  constructor(private http: HttpClient) {

  }

  public getPalestrantes(page: number, itemsPerPage: number, term: string): Observable <PaginatedResult<Palestrante[] | null>> {
    const paginatedResult: PaginatedResult<Palestrante[] | null> = new PaginatedResult<Palestrante[]>();

    let params = new HttpParams;

    if (page !== null && itemsPerPage !== null)
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());

    if (term !== null && term !== '')
      params = params.append('term', term);

    return this.http
      .get<Palestrante[]>(this.baseURL + '/all', {observe: 'response', params})
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.has('Pagination')) {
            const paginationHeader = response.headers.get('Pagination');
            paginatedResult.pagination = paginationHeader ? JSON.parse(paginationHeader) : null;
          }
          return paginatedResult;
      }));
  }

  public getPalestrante(): Observable <Palestrante[]> {
    return this.http
      .get<Palestrante[]>(`${this.baseURL}`)
      .pipe(take(1))
  }

  // public getPalestrantesById(id: number): Observable <Palestrante> {
  //   return this.http
  //     .get<Palestrante>(`${this.baseURL}/${id}`)
  //     .pipe(take(1));

  // }

  public post(): Observable <Palestrante> {
    return this.http
      .post<Palestrante>(this.baseURL,{} as Palestrante)
      .pipe(take(1));

  }

  public put(palestrante: Palestrante): Observable <Palestrante> {
    return this.http
      .put<Palestrante>(`${this.baseURL}`,palestrante)
      .pipe(take(1));

  }

  // public deletePalestrante(id: number): Observable <any> {
  //   return this.http
  //     .delete(`${this.baseURL}/${id}`)
  //     .pipe(take(1));

  // }


}
