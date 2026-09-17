import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { Observable } from "rxjs";
import { AuthenticationService } from "../../authentication.service";

export function authenticationInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    const token = inject(AuthenticationService).getAuthToken();
    
    if (token !== null) {
        return next(req.clone({
            headers: req.headers.append('Authorization', `Bearer ${token}`),
        }));
    } else {
        return next(req);
    }
}