
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private toaster: ToastrService
  ) {}

  canActivate(): boolean {
    try {
      if (localStorage.getItem('user') !== null)
        return true;
    }
    catch (e) {
      console.log('Erro ao tentar acessar o localStorage! auth guard');
      return true;
    }

    this.toaster.info('Usuário não autenticado!');
    this.router.navigate(['/user/login']);
    return false;
  }

}
