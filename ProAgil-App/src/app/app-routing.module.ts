import { ErrorComponent } from './error/error.component';
import { ContactsComponent } from './contacts/contacts.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { EventsComponent } from './events/events.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
  { path: 'events', component: EventsComponent },
  { path: 'speakers', component: SpeakersComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'contacts', component: ContactsComponent},
  { path: '', redirectTo: '', pathMatch: 'full' },  // principal
  { path: '**', component: ErrorComponent }, // se o usuário digitar rotas inexistentes
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
