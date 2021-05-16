import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  title = 'Login';
  model: any = {};

  constructor(
    private authService: AuthService, 
    public router: Router, 
    private toastr: ToastrService,
    private  spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null) {
      this.router.navigate(['/dashboard']);
    }
  }

  login() {
    this.spinner.show();

    this.model.PasswordHash = this.model.password;
    this.authService.login(this.model)
      .subscribe({
        next: () =>{
          this.router.navigate(['/dashboard']);
          this.toastr.success('Você está logado');
        },
        error: (error: any) => {
          this.toastr.error(`Falha ao fazer login: ${error}`);
        },
        complete: () => this.spinner.hide()
      });
  }

}
