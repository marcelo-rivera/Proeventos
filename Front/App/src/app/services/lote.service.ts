import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Lote } from '@app/models/Lote';

@Injectable(
//  {  providedIn: 'root' }
)
export class LoteService { [key: string]: any  // [key: string]: any para poder usar as funções com string indexando

  baseURL = 'https://localhost:5001/api/lote'

  constructor(private http: HttpClient) { }

  public getLotesByEventoId(eventoId: number): Observable <Lote[]> {
    return this.http
      .get<Lote[]>(`${this.baseURL}/${eventoId}`)
      .pipe(take(1));
  }

  public saveLote(eventoId:number , lote: Lote[]): Observable <Lote[]> {
      return this.http
      .put<Lote[]>(`${this.baseURL}/${eventoId}`,lote)
      .pipe(take(1));

  }

  public deleteLote(eventoId: number, loteId: number): Observable <any> {
    return this.http
      .delete(`${this.baseURL}/${eventoId}/${loteId}`)
      .pipe(take(1));

  }


}
