import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@app/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { debounceTime, Subject } from 'rxjs';
import { PalestranteService } from '@app/services/palestrante.service';
import { Palestrante } from '@app/models/Palestrante';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.css']
})
export class PalestranteListaComponent implements OnInit{

  modalRef?: BsModalRef;

  public palestrantes: Palestrante[] = [] ;
  //public PalestrantesFiltrados: Palestrante[] = [] ;
  public PalestranteId: number = 0;
  public pagination = {} as Pagination;

  public widthImg: number = 62;
  public marginImg: number = 2;
  public showImg: boolean = true;


  constructor(private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.pagination = {currentPage: 1, itemsPerPage: 10, totalItems: 50 } as Pagination
    this.carregarPalestrantes();
  }

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarPalestrantes(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
        filtrarPor => {
        this.spinner.show();
        this.palestranteService.getPalestrantes(this.pagination.currentPage,
                                      this.pagination.itemsPerPage,
                                      filtrarPor
          )
          .subscribe(
            (paginatedResult: PaginatedResult<Palestrante[] | null>) => {
              this.palestrantes = paginatedResult.result || [];
              //this.PalestrantesFiltrados = this.Palestrantes;
              this.pagination = paginatedResult.pagination;
            },
            (error: any) => {
              if (error.statusText != 'Unknown Error')
              {
                this.toastr.error('Erro ao carregar os Palestrantes','Erro!');
              }
            }

    // filtrarPor = filtrarPor.toLocaleLowerCase();
    // return this.eventos.filter(
    //   evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
    //   evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
          ).add(() => this.spinner.hide());
        }
      )
    }
      this.termoBuscaChanged.next(evt.value);
  }

  public carregarPalestrantes (): void {
    this.spinner.show();
    this.palestranteService.getPalestrantes(this.pagination.currentPage,
                                  this.pagination.itemsPerPage,'').subscribe(
      (paginatedResult: PaginatedResult<Palestrante[] | null>) => {
        this.palestrantes = paginatedResult.result || [];
        //this.palestrantesFiltrados = this.palestrantes;
        this.pagination = paginatedResult.pagination;
      },
      (error: any) => {
        console.log('salamão' + error.statusText);
        if (error.statusText != 'Unknown Error')
        {
          console.log(error.statusText);
          this.toastr.error('Erro ao carregar os Palestrantes','Erro!');
        }
      }
    ).add(() => this.spinner.hide());
  }

  public getImagemURL(imagemName: string): string {
    if(imagemName)
      return environment.apiURL + 'resources/perfil/' + imagemName;
    else
      return './img/nophoto.jpg';
  }

}
