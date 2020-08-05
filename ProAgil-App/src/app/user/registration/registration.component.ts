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

  constructor(public fb: FormBuilder, private tostr: ToastrService) { }

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
        confirmPasswordCtrl.setErrors(null)
      }
    }
  }

  Register() {
  }
}
