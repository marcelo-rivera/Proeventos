import { Component, OnInit } from '@angular/core';
import { UserLogin } from '@app/models/Identity/UserLogin';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
  model = {} as UserLogin;

  constructor(private accountService: AccountService,
              private router: Router,
              private toaster: ToastrService
  ){}

  ngOnInit(): void {}

  public login(): void{
    this.accountService.login(this.model).subscribe(
      () => {this.router.navigateByUrl('/dashboard');},
      (error: any) => {
        if (error.status == 401)
          this.toaster.error('usuário ou senha inválido');
        else
          console.error(error);
      }
    );
  }

}
