import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginModel } from 'ClientApp/app/models/auth/login.model';
import { AuthService } from 'ClientApp/app/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { timer } from 'rxjs';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    public myForm: FormGroup;
    private returnUrl: string;
    public errorMessage: string;
    public showValidationError: boolean = false;

    constructor(
        private authService: AuthService,
        private router: Router,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
    ) {
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'].substr(6, 10000) || '/';
        this.myForm = this.formBuilder.group({
            "email": ['', [Validators.required]],
            "password": ['', [Validators.required]],
        });
    }

    public login(formValue): void {
        const loginModel: LoginModel = {
            email: formValue.email,
            password: formValue.password
        };
        this.authService.login(loginModel).subscribe(response => {
            if (response.token) {
                localStorage.setItem('token', response.token);
                this.router.navigateByUrl(this.returnUrl);
            }
        }, error => {
            this.errorMessage = 'Incorrect email or password';
            this.showValidationError = true;
            timer(5000).subscribe(_ => {
                this.showValidationError = false;
            });
        });
    }

    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }
}
