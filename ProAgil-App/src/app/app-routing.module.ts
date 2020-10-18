import { EventEditComponent } from './events/event-edit/event-edit.component';
import { AuthGuard } from './auth/auth.guard';
import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { UserComponent } from './user/user.component';
import { EventsComponent } from './events/events.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContactsComponent } from './contacts/contacts.component';
import { ErrorComponent } from './error/error.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  {
    path: 'user', component: UserComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'registration', component: RegistrationComponent },
    ] // filhos
  },

  { path: 'events', component: EventsComponent, canActivate: [AuthGuard] },
  { path: 'event/:id/edit', component: EventEditComponent, canActivate: [AuthGuard] },
  { path: 'speakers', component: SpeakersComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'contacts', component: ContactsComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '', pathMatch: 'full' },  // principal
  { path: '**', component: ErrorComponent }, // rota curinga: se o usuário digitar rotas inexistentes. sempre fica por último
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
