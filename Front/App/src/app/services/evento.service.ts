import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { map, take } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/Pagination';

@Injectable(
//  {  providedIn: 'root'}   injeçao de dependencia (outra forma)
)
export class EventoService { [key: string]: any  // [key: string]: any para poder usar as funções com string indexando

  baseURL = environment.apiURL + 'api/evento';

  //tokenHeader = new HttpHeaders({'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user') || '{}').token}`});
  //tokenHeader = new HttpHeaders({'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxMCIsInVuaXF1ZV9uYW1lIjoidHJ1amlsbG8iLCJuYmYiOjE3MzgzMjY3NTAsImV4cCI6MTczODQxMzE1MCwiaWF0IjoxNzM4MzI2NzUwfQ.XC1JeTPyGLVYtOUjXWWpmB5dseqyp6FQEtardvXtWfci_KlU0s4Q_gisLJcPxT4R_bVRC5sYHv0bwqZjpKzxLg'});
  constructor(private http: HttpClient) {

  }

  public getEventos(page: number, itemsPerPage: number, term: string): Observable <PaginatedResult<Evento[] | null>> {
    const paginatedResult: PaginatedResult<Evento[] | null> = new PaginatedResult<Evento[]>();

    let params = new HttpParams;

    if (page !== null && itemsPerPage !== null)
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());

    if (term !== null && term !== '')
      params = params.append('term', term);

    return this.http
      .get<Evento[]>(this.baseURL, {observe: 'response', params})
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

  public getEventosByTema(tema: string): Observable <Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseURL}/${tema}/tema`)
      .pipe(take(1))
  }

  public getEventosById(id: number): Observable <Evento> {
    return this.http
      .get<Evento>(`${this.baseURL}/${id}`)
      .pipe(take(1));

  }

  public post(evento: Evento): Observable <Evento> {
    return this.http
      .post<Evento>(this.baseURL,evento)
      .pipe(take(1));

  }

  public put(evento: Evento): Observable <Evento> {
    return this.http
      .put<Evento>(`${this.baseURL}/${evento.id}`,evento)
      .pipe(take(1));

  }

  public deleteEvento(id: number): Observable <any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1));

  }

  postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file as File;
    const formData = new FormData();
    formData.append('file',fileToUpload)

    return this.http
      .post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData)
      .pipe(take(1));
  }
}
