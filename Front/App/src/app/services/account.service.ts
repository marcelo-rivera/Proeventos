import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/environment';
import { User } from '@app/models/Identity/User';
import { map, Observable, ReplaySubject, take } from 'rxjs';

@Injectable(//{
  //providedIn: 'root'
//}
)
export class AccountService {
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  public userNull = new User();


  baseUrl = environment.apiURL + 'api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user)

        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    //this.currentUserSource.complete();
  }

  public setCurrentUser(user: User | null): void {
    try{
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSource.next(user);
    } catch (e) {
      console.log('Erro ao tentar acessar o localStorage! account service');
      this.currentUserSource.next(null);
    }
  }

  public getCurrentUser(): User | null {
    const userJson = localStorage.getItem('user');
    return userJson ? JSON.parse(userJson) as User : null;
  }

  public register(model: any): Observable<User>{
    return this.http.post<User>(this.baseUrl + 'register', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

}
