import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public authService: AuthService, public router: Router, public fb: FormBuilder, private toastr: ToastrService) { }

  ngOnInit() {
  }

  loggedIn() {
    return this.authService.loggedIn();
  }
  entrar() {
    this.router.navigate(['/user/login']);
  }

  logOut() {
      localStorage.removeItem('token');
      this.toastr.show('Log Out');
      this.router.navigate(['/user/login']);
  }

}
