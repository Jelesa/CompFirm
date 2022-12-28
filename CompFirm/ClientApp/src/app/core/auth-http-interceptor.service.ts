import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "../shared/auth.service";
import { tap } from 'rxjs/operators';
import { Router } from "@angular/router";
import { ErrorService } from "../shared/error.service";

@Injectable({
  providedIn: 'root'
})
export class AuthHttpInterceptorService implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private router: Router) {
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler)
    : Observable<HttpEvent<any>> {

    if (this.authService.isUserLoggedIn()) {
      req = req.clone({ headers: req.headers.append('Authorization', `Bearer ${this.authService.getToken()}`) });
    }

    return next.handle(req).pipe(
      tap(
        _ => {
        },
        (err) => {
          if (err instanceof HttpErrorResponse) {

            if (err.error && err.error.message) {
              this.errorService.pushError(err.error.message);
            }

            if (err.status == 401) {
              console.log('Unauthorized');

              this.authService.logOut();
              this.router.navigate(['/auth/login?authFailed=true']);
            }
          }
        }
      )
    )
  }

}
