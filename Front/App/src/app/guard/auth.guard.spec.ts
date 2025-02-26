import { TestBed } from '@angular/core/testing';
import { CanActivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthenticationGuard } from './auth.guard';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

describe('authGuard', () => {
  let accountService: AccountService;
  let router: Router;
  let toastr: ToastrService;

  const executeGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) =>
    TestBed.runInInjectionContext(() => {
      const guard = new AuthenticationGuard(accountService, router, toastr);
      return guard.canActivate(route, state);
    });

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AccountService,
        { provide: Router, useValue: {} },
        { provide: ToastrService, useValue: {} }
      ]
    });
    accountService = TestBed.inject(AccountService);
    router = TestBed.inject(Router);
    toastr = TestBed.inject(ToastrService);
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
