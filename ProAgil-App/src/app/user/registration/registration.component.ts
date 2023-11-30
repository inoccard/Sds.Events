import { Router } from '@angular/router';
import { AuthService } from './../../services/auth.service';
import { User } from './../../models/User';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm: FormGroup;
  user: User;

  constructor(private authService: AuthService, private router: Router, public fb: FormBuilder, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      passwords: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', Validators.required],
      }, { validator: this.comparePassword }),
    });
  }

  comparePassword(fb: FormGroup) {
    const confirmPasswordCtrl = fb.get('confirmPassword');
    if (confirmPasswordCtrl.errors == null || 'mismatch' in confirmPasswordCtrl.errors) {
      if (fb.get('password').value !== confirmPasswordCtrl.value) {
        confirmPasswordCtrl.setErrors({ mismatch: true });
      }else{
        confirmPasswordCtrl.setErrors(null);
      }
    }
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign(
        { passwordHash: this.registerForm.get('passwords.password').value }, 
          this.registerForm.value
        );
      
        this.authService.register(this.user).subscribe(
        (newUser: any) => {
          this.router.navigate(['/user/login']);
          this.toastr.success('Cadastro Realizado');
        },
        error => {
          // tslint:disable-next-line: variable-name
          const _error = error.error;

          _error.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Cadastro Duplicado');
                break;
              default:
                this.toastr.error(`${element.code}: Erro no Cadastro`);
                break;
            }
          });
        }
      );
    }
  }
}
