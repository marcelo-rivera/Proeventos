import { Inject, Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateFn } from '@angular/router';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

// export const authGuard: CanActivateFn = (route, state) => {
//   const router = Inject(Router);
//   const toaster = Inject(ToastrService);
//   const accountService = Inject(AccountService);

// //    if (localStorage.getItem('user') !== null){
//     if (accountService.getCurrentUser !== null){
//       return true;
//     } else {
//       toaster.info('Usuário não autenticado!');
//       router.navigate(['/user/login']);
//       return false;
//     }

// }
@Injectable({
  providedIn: 'root'
})

export class AuthenticationGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router, private toaster: ToastrService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    try {
      if (this.accountService.getCurrentUser() !== null) {
        return true;
      } else {
        this.toaster.info('Usuário não autenticado!');
        this.router.navigate(['/user/login']);
        return false;
      }
    } catch (error) {
      //this.toaster.error('Erro ao tentar autenticar o usuário!');
      return false;
    }
  }
}
