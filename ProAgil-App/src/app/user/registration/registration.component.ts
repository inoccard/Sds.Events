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
      email: ['', Validators.required, Validators.email],
      userName: ['', Validators.required],
      passwords: this.fb.group({
        password: ['', Validators.required, Validators.minLength(4)],
        confirmPassword: ['', Validators.required],
      }),
      
    });
  }

  Register() {
    
  }
}
