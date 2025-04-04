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

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrl: './evento-lista.component.scss'
})
export class EventoListaComponent implements OnInit{


  modalRef?: BsModalRef;


  public eventos: Evento[] = [] ;
  //public eventosFiltrados: Evento[] = [] ;
  public eventoId: number = 0;
  public pagination = {} as Pagination;

  public widthImg: number = 62;
  public marginImg: number = 2;
  public showImg: boolean = true;

  // private _filtrolista: string= '';

  // public get filtrolista(): string{
  //   return this._filtrolista;
  // }

  // public set filtrolista(value: string){
  //   this._filtrolista = value;
  //   this.eventosFiltrados = this.filtrolista ? this.filtrarEventos(this.filtrolista) : this.eventos;
  // }

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
        filtrarPor => {
        this.spinner.show();
        this.eventoService.getEventos(this.pagination.currentPage,
                                      this.pagination.itemsPerPage,
                                      filtrarPor
          )
          .subscribe(
            (paginatedResult: PaginatedResult<Evento[] | null>) => {
              this.eventos = paginatedResult.result || [];
              //this.eventosFiltrados = this.eventos;
              this.pagination = paginatedResult.pagination;
            },
            (error: any) => {
              this.spinner.hide();
              if (error.statusText != 'Unknown Error')
              {
                this.toastr.error('Erro ao carregar os eventos','Erro!');
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

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.pagination = {currentPage: 1, itemsPerPage: 1, totalItems: 1 } as Pagination
    this.carregarEventos();
  }

  public showImage() {
    this.showImg = !this.showImg;
  }

  public mostraImagem(imagemURL: string): string{

    return (imagemURL != ''
      ? `${environment.apiURL}resources/images/${imagemURL}`
      : '/img/nophoto.jpg'
    );
  }

  public carregarEventos (): void {
    this.spinner.show();

    this.eventoService.getEventos(this.pagination.currentPage,
                                  this.pagination.itemsPerPage,'').subscribe(
      (paginatedResult: PaginatedResult<Evento[] | null>) => {
        this.eventos = paginatedResult.result || [];
        //this.eventosFiltrados = this.eventos;
        this.pagination = paginatedResult.pagination;
      },
      (error: any) => {
        this.spinner.hide();
        if (error.statusText != 'Unknown Error')
        {
          this.toastr.error('Erro ao carregar os eventos','Erro!');
        }
      },
    ).add(() => this.spinner.hide());
  }

  openModal(event: any, template: TemplateRef<void>, eventoId: number) {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        console.log(result.message);
        if (result.message === 'Excluído') {
          this.toastr.success('Evento excluído !', 'Exclusão de Evento')
        //  this.spinner.hide();
          this.carregarEventos();
        }
      },
      (error: any) => {
        console.error(error);
        this.toastr.error(`Erro ao tentar deletar o evento ${this.eventoId}`, 'Erro');
      //  this.spinner.hide();
      },
      //() => {this.spinner.hide()},
    ).add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`])
  }


}
