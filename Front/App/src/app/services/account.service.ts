import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@app/environment';
import { User } from '@app/models/Identity/User';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
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
      //console.log('Erro ao tentar acessar o localStorage! account service');
      this.currentUserSource.next(null);
    }
  }

  getUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(this.baseUrl + 'getUser').pipe(take(1));
  }

  updateUser(model: UserUpdate): Observable<void> {
    //console.log(model);
    return this.http.put<UserUpdate>(this.baseUrl + 'updateUser', model).pipe(
      take(1),
      map((user: UserUpdate) => {
        this.setCurrentUser(user);
      })
    )
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

  postUpload(file: File): Observable<UserUpdate> {
    const fileToUpload = file as File;
    const formData = new FormData();
    formData.append('file',fileToUpload)

    return this.http
      .post<UserUpdate>(`${this.baseUrl}upload-image`, formData)
      .pipe(take(1));
  }

}
