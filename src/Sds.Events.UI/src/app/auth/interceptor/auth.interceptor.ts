import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/internal/operators/tap';

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (localStorage.getItem('token') != null) {
      const cloneRequest = request.clone(({
        headers: request.headers.set('Authorization', `Bearer ${localStorage.getItem('token')}`)
      }));
      return next.handle(cloneRequest).pipe(
        tap(
          succ => { },
          error => {
            // se for não autorizado, redireciona para tela de login
            if (error.status === 401) {
              this.router.navigateByUrl('user/login');
            }
          }
        )
      );
    }
    else
      return next.handle(request);
  }
}
