import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { PalestranteService } from '@app/services/palestrante.service';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.css']
})
export class PerfilDetalheComponent implements OnInit {

  userUpdate =  {} as UserUpdate;

  @Output() changeFormValue = new EventEmitter();

  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder,
    private accountService: AccountService,
    private palestranteService: PalestranteService,
    private toastr: ToastrService,
    private router: Router,
    private spinner : NgxSpinnerService
  ) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  private verificaForm(): void {
    this.form.valueChanges
      .subscribe(() =>
        this.changeFormValue.emit({... this.form.value})
      );
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        //console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toastr.success('Usuário carregado com sucesso!', 'Sucesso');
      },
      (error) => {
        //console.error(error);
        //this.toastr.error('Erro ao tentar carregar usuário.', 'Erro');
        this.router.navigate(['/dashboard']);
      }
    )
    .add(() => this.spinner.hide());
  }

  public validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password','confirmePassword')
    };

    this.form = this.fb.group({
    userName: [''],
    imagemURL: [''],
    titulo: ['',Validators.required],
    primeiroNome: ['',[Validators.required,Validators.minLength(8),Validators.maxLength(50)]],
    ultimoNome: ['',[Validators.required,Validators.minLength(8),Validators.maxLength(50)]],
    email: ['',[Validators.required,Validators.email]],
    phoneNumber: ['',[Validators.required,Validators.minLength(11),Validators.maxLength(12)]],
    descricao: ['',[Validators.required,Validators.maxLength(30)]],
    funcao: ['',[Validators.required]],
    password: ['',Validators.minLength(6)],
    confirmePassword: ['']
    }, formOptions);
  }

  onSubmit(): void {
    // Vai parar aqui se o form estiver inválido
    if (this.form.invalid) {
      return;
    }else{
      this.atualizarUsuario();
    }
  }

  public atualizarUsuario(): void {
    this.spinner.show();
    this.userUpdate = {... this.form.value};
    this.userUpdate.token = "A";//"eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxMCIsInVuaXF1ZV9uYW1lIjoidHJ1amlsbG8iLCJuYmYiOjE3NDEzNzE1OTksImV4cCI6MTc0MTQ1Nzk5OSwiaWF0IjoxNzQxMzcxNTk5fQ.y0srYuzNJFkM8tJqozDtpB6wHMZZCP1gR6zIQsYAb_b-BVByLrgp8HlwgDRzWvym-WFngfd_ckU-LvkGehVPYw";
    //console.log(this.userUpdate);
    if (this.f.funcao.value == "Palestrante") {
      this.palestranteService.post().subscribe(
        () => this.toastr.success('Função palestrante Ativada!','Sucesso'),
        (error) => {
          this.toastr.error('Erro ao tentar ativar função palestrante.', 'Erro');
          console.error(error);
        })
    }

    this.userUpdate.imagemURL = "";
    this.accountService.updateUser(this.userUpdate).subscribe(
      () => {
        this.toastr.success('Usuário atualizado com sucesso!', 'Sucesso');
        this.router.navigate(['/dashboard']);
      },
      (error) => {
        console.error(error);
        this.toastr.error('Erro ao tentar atualizar usuário.', 'Erro');
      }
    )
    .add(() => this.spinner.hide());
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }


}
