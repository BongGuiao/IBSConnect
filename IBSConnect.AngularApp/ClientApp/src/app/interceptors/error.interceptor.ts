import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpEvent, HttpErrorResponse, HttpHandler } from "@angular/common/http";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { ErrorService } from "../services/error.service";
import { Router } from "@angular/router";
import { AuthenticationService } from "../services/authentication.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private errorService: ErrorService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(tap(response => {

    }, (response: any) => {
      console.log(response)
      if (response instanceof HttpErrorResponse) {
        if (response.status === 401 && response.error) {
          this.errorService.addErrors([response.error.message]);
          return;
        }

        if (response.status === 403 && response.error) {
          this.authenticationService.logoutUser();
          this.router.navigate(["/session-expired"]);
          return;
        }

        if (response.status === 400 && response.error) {
          if (response.error.messages) {
            this.errorService.addErrors(response.error.messages);
          }
          if (response.error.message) {
            this.errorService.addErrors([response.error.message]);
          }
          return;
        }

        if (response.status === 500 && response.error) {
          if (response.error.type != 'authentication') {
            if (response.error.messages) {
              this.errorService.addErrors(response.error.messages);
            } else {
              this.errorService.addErrors([response.error.message]);
            }
          }
          return;
        }

        this.errorService.addErrors([`An error has occurred`]);
      }
    }));
  }
}
